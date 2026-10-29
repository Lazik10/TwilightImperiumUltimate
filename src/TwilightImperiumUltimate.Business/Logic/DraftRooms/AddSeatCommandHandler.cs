using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Adds a seat when an authorized room manager requests it.
/// </summary>
public sealed class AddSeatCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog)
    : IRequestHandler<AddSeatCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    /// <inheritdoc />
    public async Task<Result> Handle(AddSeatCommand request, CancellationToken cancellationToken)
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

                    if (scope.Room.Status != DraftRoomStatus.Lobby)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.RoomChanged));
                    }

                    if (!this._authorizer.CanManageSeats(actor))
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.Forbidden));
                    }

                    if (scope.Room.Seats.Count >= DraftRoomLimits.MaxSeatCount)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.SeatLimitReached));
                    }

                    var nextIndex = scope.Room.Seats
                        .Select(seat => seat.Index)
                        .DefaultIfEmpty(0)
                        .Max() + 1;
                    var seat = new DraftRoomSeat
                    {
                        Id = Guid.NewGuid(),
                        RoomId = scope.Room.Id,
                        Index = nextIndex,
                        Room = scope.Room,
                    };

                    scope.Context.DraftRoomSeats.Add(seat);
                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.SeatAdded,
                        actor.Participant.Id,
                        new { seat.Id, seat.Index });

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
