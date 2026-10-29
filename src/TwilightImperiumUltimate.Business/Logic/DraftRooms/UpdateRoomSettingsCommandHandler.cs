using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Updates room settings when an authorized manager requests it in the lobby.
/// </summary>
public sealed class UpdateRoomSettingsCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog,
    IDraftTypeRegistry registry,
    IDraftRoomDraftPersistence persistence)
    : IRequestHandler<UpdateRoomSettingsCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    private readonly IDraftTypeRegistry _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    private readonly IDraftRoomDraftPersistence _persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateRoomSettingsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

        var roomName = request.Name?.Trim();
        if (string.IsNullOrWhiteSpace(roomName)
            || roomName.Length < DraftRoomLimits.MinRoomNameLength
            || roomName.Length > DraftRoomLimits.MaxRoomNameLength)
        {
            return Failure(DraftRoomErrorCode.InvalidRoomName);
        }

        try
        {
            return await this._unitOfWork.ExecuteAsync(
                request.Code,
                async scope =>
                {
                    var actor = this._authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
                    }

                    if (scope.Room.Status != DraftRoomStatus.Lobby)
                    {
                        return Failure(DraftRoomErrorCode.RoomChanged);
                    }

                    if (!this._authorizer.CanChangeSettings(actor))
                    {
                        return Failure(DraftRoomErrorCode.Forbidden);
                    }

                    if (request.DraftType is DraftTypeId type)
                    {
                        var change = await DraftRoomTypeChange.ApplyAsync(
                            scope, actor, type, this._registry, this._persistence, this._draftRoomLog, cancellationToken);
                        if (change.IsFailed)
                        {
                            return change;
                        }
                    }

                    var previousName = scope.Room.Name;
                    var wasLocked = scope.Room.IsLocked;
                    if (!string.Equals(previousName, roomName, StringComparison.Ordinal))
                    {
                        scope.Room.Name = roomName;
                        this._draftRoomLog.Append(
                            scope,
                            DraftLogEntryType.RoomRenamed,
                            actor.Participant.Id,
                            new { PreviousName = previousName, NewName = roomName });
                    }

                    scope.Room.IsLocked = request.IsLocked;
                    if (wasLocked != request.IsLocked)
                    {
                        this._draftRoomLog.Append(
                            scope,
                            request.IsLocked ? DraftLogEntryType.RoomLocked : DraftLogEntryType.RoomUnlocked,
                            actor.Participant.Id,
                            new { IsLocked = request.IsLocked });
                    }

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

    private static Result Failure(DraftRoomErrorCode code)
    {
        return Result.Fail(new DraftRoomError(code));
    }
}
