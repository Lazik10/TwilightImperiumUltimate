using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Enforces current membership under the room gate, or atomic owner-only deletion of an orphaned result.</summary>
public sealed class DeleteResultCommandHandler(
    IDraftResultRepository repository,
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog log) : IRequestHandler<DeleteResultCommand, Result>
{
    /// <inheritdoc />
    public async Task<Result> Handle(DeleteResultCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var result = request.Id == Guid.Empty ? null : await repository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return Failure(DraftRoomErrorCode.ResultNotFound);
        }

        var code = await repository.GetSourceRoomCodeAsync(result.SourceRoomId, cancellationToken);
        if (code is null)
        {
            return await this.DeleteOrphanAsync(request, cancellationToken);
        }

        try
        {
            return await unitOfWork.ExecuteAsync(
                code,
                async scope =>
                {
                    var actor = authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null || !authorizer.CanManageDraft(actor))
                    {
                        return Failure(DraftRoomErrorCode.Forbidden);
                    }

                    var tracked = await repository.LoadForRoomAsync(scope, request.Id, cancellationToken);
                    if (tracked is null)
                    {
                        return Failure(DraftRoomErrorCode.ResultNotFound);
                    }

                    scope.Context.DraftResults.Remove(tracked);
                    if (scope.Room.ResultId == tracked.Id)
                    {
                        scope.Room.ResultId = null;
                    }

                    log.Append(scope, DraftLogEntryType.DraftResultDeleted, actor.Participant.Id, new { });
                    return Result.Ok();
                },
                cancellationToken);
        }
        catch (DraftRoomNotFoundException)
        {
            return await this.DeleteOrphanAsync(request, cancellationToken);
        }
        catch (DraftRoomConflictException)
        {
            return Failure(DraftRoomErrorCode.RoomChanged);
        }
    }

    private static Result Failure(DraftRoomErrorCode code) => Result.Fail(new DraftRoomError(code));

    private async Task<Result> DeleteOrphanAsync(DeleteResultCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Caller.UserId))
        {
            return Failure(DraftRoomErrorCode.Forbidden);
        }

        return await repository.DeleteOwnedOrphanAsync(request.Id, request.Caller.UserId, cancellationToken)
            ? Result.Ok()
            : Failure(DraftRoomErrorCode.Forbidden);
    }
}
