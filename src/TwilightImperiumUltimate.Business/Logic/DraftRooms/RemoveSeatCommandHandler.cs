using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Removes and renumbers a free seat when an authorized room manager requests it.
/// </summary>
public sealed class RemoveSeatCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog)
    : IRequestHandler<RemoveSeatCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    /// <inheritdoc />
    public async Task<Result> Handle(RemoveSeatCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

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

                    if (!this._authorizer.CanManageSeats(actor))
                    {
                        return Failure(DraftRoomErrorCode.Forbidden);
                    }

                    if (scope.Room.Seats.Count <= DraftRoomLimits.MinSeatCount)
                    {
                        return Failure(DraftRoomErrorCode.MinimumSeatCountReached);
                    }

                    var seat = scope.Room.Seats.SingleOrDefault(item => item.Id == request.SeatId);
                    if (seat is null)
                    {
                        return Failure(DraftRoomErrorCode.SeatNotFound);
                    }

                    if (seat.ParticipantId is not null)
                    {
                        return Failure(DraftRoomErrorCode.SeatOccupied);
                    }

                    var remainingSeats = scope.Room.Seats
                        .Where(item => item.Id != seat.Id)
                        .OrderBy(item => item.Index)
                        .ToArray();

                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.SeatRemoved,
                        actor.Participant.Id,
                        new { seat.Id, seat.Index });
                    scope.Context.DraftRoomSeats.Remove(seat);
                    scope.Room.Seats.Remove(seat);

                    // Stage outside the valid index range so compaction cannot collide with the unique index.
                    for (var index = 0; index < remainingSeats.Length; index++)
                    {
                        remainingSeats[index].Index = DraftRoomLimits.MaxSeatCount + index + 1;
                    }

                    await scope.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    for (var index = 0; index < remainingSeats.Length; index++)
                    {
                        remainingSeats[index].Index = index + 1;
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
