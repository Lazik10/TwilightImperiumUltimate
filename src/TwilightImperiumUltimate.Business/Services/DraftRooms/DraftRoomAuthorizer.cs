using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Enforces draft-room identity verification and role-based permission policies.
/// </summary>
public sealed class DraftRoomAuthorizer(IGuestSecretService guestSecretService)
    : IDraftRoomAuthorizer
{
    private readonly IGuestSecretService _guestSecretService = guestSecretService
        ?? throw new ArgumentNullException(nameof(guestSecretService));

    /// <inheritdoc />
    public DraftRoomActor? Resolve(DraftRoom room, DraftRoomCaller caller)
    {
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(caller);

        if (!DraftRoomCallerValidator.IsValid(caller))
        {
            return null;
        }

        DraftRoomParticipant? participant;
        if (caller.UserId is not null)
        {
            participant = room.Participants.SingleOrDefault(item => item.UserId == caller.UserId);
        }
        else if (caller.GuestParticipantId is Guid participantId && caller.GuestSecret is not null)
        {
            participant = room.Participants.SingleOrDefault(
                item => item.Id == participantId && item.UserId is null);

            if (participant?.SecretHash is null
                || !this._guestSecretService.Verify(caller.GuestSecret, participant.SecretHash))
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        if (participant is null || participant.Status != DraftParticipantStatus.Active)
        {
            return null;
        }

        var seat = room.Seats.SingleOrDefault(item => item.ParticipantId == participant.Id);

        return new DraftRoomActor(room, participant, seat);
    }

    /// <inheritdoc />
    public bool CanManageSeats(DraftRoomActor actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        return IsActiveActor(actor)
            && IsManager(actor)
            && actor.Room.Status == DraftRoomStatus.Lobby;
    }

    /// <inheritdoc />
    public bool CanChangeSettings(DraftRoomActor actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        return this.CanManageSeats(actor);
    }

    /// <inheritdoc />
    public bool CanManageDraft(DraftRoomActor actor)
    {
        ArgumentNullException.ThrowIfNull(actor);
        return IsActiveActor(actor) && IsManager(actor);
    }

    /// <inheritdoc />
    public bool CanKick(DraftRoomActor actor, DraftRoomParticipant target)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(target);

        if (!IsActiveActor(actor)
            || !IsManager(actor)
            || !IsActiveParticipantInRoom(actor, target)
            || target.Id == actor.Participant.Id
            || target.Role == DraftRoomRole.Host)
        {
            return false;
        }

        return actor.Role == DraftRoomRole.Host || target.Role == DraftRoomRole.Member;
    }

    /// <inheritdoc />
    public bool CanChangeRoles(DraftRoomActor actor, DraftRoomParticipant target)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(target);

        return IsActiveActor(actor)
            && actor.Role == DraftRoomRole.Host
            && IsActiveParticipantInRoom(actor, target)
            && target.Role != DraftRoomRole.Host
            && target.Id != actor.Participant.Id;
    }

    /// <inheritdoc />
    public bool CanClose(DraftRoomActor actor)
    {
        return this.CanManageDraft(actor);
    }

    /// <inheritdoc />
    public bool CanClaimSeat(DraftRoomActor actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        return IsActiveActor(actor)
            && (actor.Room.Status == DraftRoomStatus.Lobby
                || (actor.Room.Status == DraftRoomStatus.InProgress
                    && actor.Seat is null
                    && actor.Room.Seats.Any(seat => this.CanClaimSeat(actor, seat))));
    }

    /// <inheritdoc />
    public bool CanClaimSeat(DraftRoomActor actor, DraftRoomSeat target)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(target);
        if (!IsActiveActor(actor) || target.RoomId != actor.Room.Id || target.ParticipantId is not null)
        {
            return false;
        }

        if (actor.Room.Status == DraftRoomStatus.Lobby)
        {
            return true;
        }

        return actor.Room.Status == DraftRoomStatus.InProgress
            && actor.Seat is null
            && target.HasBeenReleased
            && (actor.Participant.DraftSeatId is null
                || (actor.Participant.DraftSeatId == target.Id && actor.Participant.CanReclaimDraftSeat));
    }

    /// <inheritdoc />
    public bool CanUnclaimSeat(DraftRoomActor actor, DraftRoomSeat target)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(target);

        if (target.RoomId != actor.Room.Id
            || !actor.Room.Seats.Any(seat => seat.Id == target.Id)
            || target.ParticipantId is null
            || !IsActiveActor(actor))
        {
            return false;
        }

        return actor.Room.Status != DraftRoomStatus.Finished
            && (IsManager(actor)
            || ((actor.Room.Status is DraftRoomStatus.Lobby or DraftRoomStatus.InProgress)
                && target.ParticipantId == actor.Participant.Id));
    }

    private static bool IsManager(DraftRoomActor actor)
    {
        return actor.Role is DraftRoomRole.Host or DraftRoomRole.CoHost;
    }

    private static bool IsActiveActor(DraftRoomActor actor)
    {
        return actor.Participant.Status == DraftParticipantStatus.Active
            && actor.Room.Participants.Any(participant => participant.Id == actor.Participant.Id);
    }

    private static bool IsActiveParticipantInRoom(DraftRoomActor actor, DraftRoomParticipant target)
    {
        return target.RoomId == actor.Room.Id
            && target.Status == DraftParticipantStatus.Active
            && actor.Room.Participants.Any(participant => participant.Id == target.Id);
    }
}
