using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Deletes the room and its persisted participants and seats when an authorized manager requests it.
/// </summary>
public sealed class CloseRoomCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer)
    : IRequestHandler<CloseRoomCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    /// <inheritdoc />
    public async Task<Result> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

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

                    if (!this._authorizer.CanClose(actor))
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.Forbidden));
                    }

                    scope.Context.DraftRooms.Remove(scope.Room);

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
