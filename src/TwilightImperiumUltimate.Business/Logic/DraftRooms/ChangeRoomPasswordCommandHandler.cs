using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Replaces the room password hash when an authorized manager requests it.
/// </summary>
public sealed class ChangeRoomPasswordCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IRoomPasswordHasher roomPasswordHasher,
    IDraftRoomLog draftRoomLog)
    : IRequestHandler<ChangeRoomPasswordCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IRoomPasswordHasher _roomPasswordHasher = roomPasswordHasher
        ?? throw new ArgumentNullException(nameof(roomPasswordHasher));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    /// <inheritdoc />
    public async Task<Result> Handle(ChangeRoomPasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

        if (string.IsNullOrEmpty(request.Password)
            || request.Password.Length < DraftRoomLimits.MinPasswordLength
            || request.Password.Length > DraftRoomLimits.MaxPasswordLength)
        {
            return Failure(DraftRoomErrorCode.InvalidPassword);
        }

        try
        {
            return await this._unitOfWork.ExecuteAsync(
                request.Code,
                scope =>
                {
                    var actor = this._authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.NotFoundOrWrongPassword));
                    }

                    if (scope.Room.Status != DraftRoomStatus.Lobby)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.RoomChanged));
                    }

                    if (!this._authorizer.CanChangeSettings(actor))
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.Forbidden));
                    }

                    scope.Room.PasswordHash = this._roomPasswordHasher.Hash(scope.Room, request.Password);
                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.PasswordChanged,
                        actor.Participant.Id,
                        new { Changed = true });

                    return Task.FromResult(Result.Ok());
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
