using System.Globalization;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Converts a verified guest participant into an account-backed participant.
/// </summary>
public sealed class LinkAccountCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IGuestSecretService guestSecretService,
    IDraftRoomDisplayNameValidator displayNameValidator,
    IDraftRoomLog draftRoomLog)
    : IRequestHandler<LinkAccountCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IGuestSecretService _guestSecretService = guestSecretService
        ?? throw new ArgumentNullException(nameof(guestSecretService));

    private readonly IDraftRoomDisplayNameValidator _displayNameValidator = displayNameValidator
        ?? throw new ArgumentNullException(nameof(displayNameValidator));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    /// <inheritdoc />
    public async Task<Result> Handle(LinkAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

        if (!IsValidCaller(request.Caller))
        {
            return Failure(DraftRoomErrorCode.InvalidCaller);
        }

        try
        {
            return await this._unitOfWork.ExecuteAsync(
                request.Code,
                async scope =>
                {
                    var guestParticipant = scope.Room.Participants.SingleOrDefault(
                        participant => participant.Id == request.Caller.GuestParticipantId
                            && participant.UserId is null);

                    if (guestParticipant is null
                        || guestParticipant.SecretHash is null
                        || !this._guestSecretService.Verify(
                            request.Caller.GuestSecret!,
                            guestParticipant.SecretHash))
                    {
                        return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
                    }

                    if (guestParticipant.Status == DraftParticipantStatus.Kicked)
                    {
                        return Failure(DraftRoomErrorCode.ParticipantKicked);
                    }

                    if (guestParticipant.Status != DraftParticipantStatus.Active)
                    {
                        return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
                    }

                    if (scope.Room.Participants.Any(
                        participant => participant.UserId == request.Caller.UserId))
                    {
                        return Failure(DraftRoomErrorCode.AccountAlreadyInRoom);
                    }

                    var displayName = FindAvailableDisplayName(
                        scope.Room.Participants,
                        guestParticipant.Id,
                        request.Caller.UserName!);
                    if (displayName is null)
                    {
                        return Failure(DraftRoomErrorCode.DuplicateDisplayName);
                    }

                    var previousDisplayName = guestParticipant.DisplayName;
                    guestParticipant.UserId = request.Caller.UserId;
                    guestParticipant.SecretHash = null;
                    guestParticipant.DisplayName = displayName;
                    guestParticipant.DisplayNameKey = this._displayNameValidator.MakeKey(displayName);

                    if (scope.Room.ResultId is Guid resultId)
                    {
                        var result = await scope.Context.DraftResults.AsTracking().Include(item => item.Players)
                            .SingleOrDefaultAsync(item => item.Id == resultId && item.SourceRoomId == scope.Room.Id, cancellationToken);
                        if (result is not null)
                        {
                            var seat = scope.Room.Seats.SingleOrDefault(item => item.ParticipantId == guestParticipant.Id);
                            var player = result.Players.SingleOrDefault(item => item.SeatIndex == seat?.Index);
                            if (player is not null && player.UserId is null)
                            {
                                player.UserId = request.Caller.UserId;
                            }

                            if (guestParticipant.Role == DraftRoomRole.Host && result.OwnerUserId is null)
                            {
                                result.OwnerUserId = request.Caller.UserId;
                            }
                        }
                    }

                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.ParticipantLinked,
                        guestParticipant.Id,
                        new
                        {
                            guestParticipant.Id,
                            PreviousDisplayName = previousDisplayName,
                            NewDisplayName = guestParticipant.DisplayName,
                            Role = guestParticipant.Role.ToString(),
                        });

                    return Result.Ok();
                },
                cancellationToken).ConfigureAwait(false);
        }
        catch (DraftRoomNotFoundException)
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }
        catch (DraftRoomConflictException)
        {
            return Failure(DraftRoomErrorCode.RoomChanged);
        }
    }

    private static bool IsValidCaller(DraftRoomCaller caller)
    {
        return !string.IsNullOrWhiteSpace(caller.UserId)
            && !string.IsNullOrWhiteSpace(caller.UserName)
            && caller.UserName.Length <= DraftRoomLimits.MaxAccountUserNameLength
            && caller.GuestParticipantId is Guid participantId
            && participantId != Guid.Empty
            && !string.IsNullOrWhiteSpace(caller.GuestSecret);
    }

    private static Result Failure(DraftRoomErrorCode code)
    {
        return Result.Fail(new DraftRoomError(code));
    }

    private string? FindAvailableDisplayName(
        IEnumerable<DraftRoomParticipant> participants,
        Guid guestParticipantId,
        string userName)
    {
        var activeKeys = participants
            .Where(participant => participant.Id != guestParticipantId
                && participant.Status == DraftParticipantStatus.Active)
            .Select(participant => participant.DisplayNameKey)
            .ToHashSet(StringComparer.Ordinal);

        for (var suffix = 0; suffix <= DraftRoomLimits.MaxActiveParticipants; suffix++)
        {
            var candidate = suffix == 0
                ? userName
                : string.Concat(userName, (suffix + 1).ToString(CultureInfo.InvariantCulture));
            if (!activeKeys.Contains(this._displayNameValidator.MakeKey(candidate)))
            {
                return candidate;
            }
        }

        return null;
    }
}
