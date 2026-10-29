using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Loads an authorized draft-room view without exposing participant credentials or password hashes.
/// </summary>
public sealed class GetDraftRoomViewQueryHandler(
    IDraftRoomRepository draftRoomRepository,
    IDraftRoomAuthorizer authorizer,
    IPresenceTracker presenceTracker,
    IDraftRoomDraftViewProvider draftViewProvider)
    : IRequestHandler<GetDraftRoomViewQuery, Result<DraftRoomViewDto>>
{
    private readonly IDraftRoomRepository _draftRoomRepository = draftRoomRepository
        ?? throw new ArgumentNullException(nameof(draftRoomRepository));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IPresenceTracker _presenceTracker = presenceTracker
        ?? throw new ArgumentNullException(nameof(presenceTracker));

    private readonly IDraftRoomDraftViewProvider _draftViewProvider = draftViewProvider
        ?? throw new ArgumentNullException(nameof(draftViewProvider));

    /// <inheritdoc />
    public async Task<Result<DraftRoomViewDto>> Handle(
        GetDraftRoomViewQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Caller);

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var code = request.Code.Trim().ToUpperInvariant();
        var room = await this._draftRoomRepository
            .GetRoomForViewByCodeAsync(code, cancellationToken)
            .ConfigureAwait(false);
        if (room is null)
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var actor = this._authorizer.Resolve(room, request.Caller);
        if (actor is null)
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var seatIdsByParticipantId = room.Seats
            .Where(seat => seat.ParticipantId is not null)
            .ToDictionary(seat => seat.ParticipantId.GetValueOrDefault(), seat => seat.Id);
        var activeParticipants = room.Participants
            .Where(participant => participant.Status == DraftParticipantStatus.Active)
            .OrderBy(participant => participant.DisplayNameKey, StringComparer.Ordinal)
            .Select(participant => new DraftRoomViewParticipantDto
            {
                Id = participant.Id,
                DisplayName = participant.DisplayName,
                Role = participant.Role,
                IsGuest = participant.UserId is null,
                IsOnline = this._presenceTracker.IsOnline(room.Id, participant.Id),
                SeatId = seatIdsByParticipantId.GetValueOrDefault(participant.Id),
            })
            .ToArray();

        var draftView = await this._draftViewProvider.GetAsync(actor, cancellationToken);
        var view = new DraftRoomViewDto
        {
            RoomId = room.Id,
            Code = room.Code,
            Name = room.Name,
            Status = room.Status,
            IsLocked = room.IsLocked,
            Version = room.Version,
            DraftType = draftView.Type,
            Settings = draftView.Settings,
            Draft = draftView.Draft,
            CanStart = draftView.CanStart,
            StartBlockers = draftView.StartBlockers,
            CanReset = draftView.CanReset,
            Participants = activeParticipants,
            Seats = room.Seats
                .OrderBy(seat => seat.Index)
                .Select(seat => new DraftRoomViewSeatDto
                {
                    Id = seat.Id,
                    Index = seat.Index,
                    ParticipantId = seat.ParticipantId,
                    CanClaim = this._authorizer.CanClaimSeat(actor, seat),
                    CanReclaim = room.Status == DraftRoomStatus.InProgress
                        && actor.Participant.CanReclaimDraftSeat && actor.Participant.DraftSeatId == seat.Id
                        && this._authorizer.CanClaimSeat(actor, seat),
                    CanUnclaim = this._authorizer.CanUnclaimSeat(actor, seat),
                    CanAssign = this._authorizer.CanManageDraft(actor)
                        && seat.ParticipantId is null
                        && (room.Status == DraftRoomStatus.Lobby || (room.Status == DraftRoomStatus.InProgress && seat.HasBeenReleased)),
                    AssignableParticipantIds = this._authorizer.CanManageDraft(actor)
                        ? room.Participants
                            .Where(participant => !room.Seats.Any(item => item.ParticipantId == participant.Id))
                            .Where(participant => this._authorizer.CanClaimSeat(new DraftRoomActor(room, participant, null), seat))
                            .Select(participant => participant.Id)
                            .ToArray()
                        : [],
                })
                .ToArray(),
            Me = new DraftRoomViewMeDto
            {
                ParticipantId = actor.Participant.Id,
                Role = actor.Role,
                SeatId = actor.Seat?.Id,
                CanManageSeats = this._authorizer.CanManageSeats(actor),
                CanChangeSettings = this._authorizer.CanChangeSettings(actor),
                CanKick = room.Participants.Any(participant => this._authorizer.CanKick(actor, participant)),
                CanChangeRoles = room.Participants.Any(participant => this._authorizer.CanChangeRoles(actor, participant)),
                CanClose = this._authorizer.CanClose(actor),
                CanClaimSeat = this._authorizer.CanClaimSeat(actor),
            },
        };

        return Result.Ok(view);
    }

    private static Result<DraftRoomViewDto> Failure(DraftRoomErrorCode code)
    {
        return Result.Fail<DraftRoomViewDto>(new DraftRoomError(code));
    }
}
