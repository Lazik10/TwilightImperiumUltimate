using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Deletes runtime state and returns the room to its lobby in one transaction.</summary>
public sealed class ResetDraftCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomDraftPersistence persistence,
    IDraftRoomLog log) : IRequestHandler<ResetDraftCommand, Result>
{
    private readonly IDraftRoomUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IDraftRoomAuthorizer authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));
    private readonly IDraftRoomDraftPersistence persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
    private readonly IDraftRoomLog log = log ?? throw new ArgumentNullException(nameof(log));

    /// <inheritdoc />
    public async Task<Result> Handle(ResetDraftCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);
        try
        {
            return await this.unitOfWork.ExecuteAsync(
                request.Code,
                async scope =>
                {
                    var actor = this.authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword));
                    }

                    if (!this.authorizer.CanManageDraft(actor))
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.Forbidden));
                    }

                    if (scope.Room.Status != DraftRoomStatus.InProgress)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.RoomChanged));
                    }

                    await this.persistence.DeleteStateAsync(scope, cancellationToken);
                    foreach (var participant in scope.Room.Participants)
                    {
                        participant.DraftSeatId = null;
                        participant.CanReclaimDraftSeat = false;
                    }

                    foreach (var seat in scope.Room.Seats)
                    {
                        seat.HasBeenReleased = false;
                    }

                    scope.Room.Status = DraftRoomStatus.Lobby;
                    this.log.Append(scope, DraftLogEntryType.DraftReset, actor.Participant.Id, new { scope.Room.DraftType });
                    return Result.Ok();
                },
                cancellationToken);
        }
        catch (DraftRoomNotFoundException)
        {
            return Result.Fail(new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword));
        }
        catch (DraftRoomConflictException)
        {
            return Result.Fail(new DraftRoomError(DraftRoomErrorCode.RoomChanged));
        }
    }
}
