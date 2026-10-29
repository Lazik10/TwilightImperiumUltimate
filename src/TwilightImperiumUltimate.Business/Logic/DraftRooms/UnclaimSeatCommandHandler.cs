using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Unclaims a participant's seat when allowed by the room's permission policy.
/// </summary>
public sealed class UnclaimSeatCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog)
    : IRequestHandler<UnclaimSeatCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    /// <inheritdoc />
    public async Task<Result> Handle(UnclaimSeatCommand request, CancellationToken cancellationToken)
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

                    var seat = scope.Room.Seats.SingleOrDefault(item => item.Id == request.SeatId);
                    if (seat is null)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.SeatNotFound));
                    }

                    if (seat.ParticipantId is null)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.SeatNotOccupied));
                    }

                    if (!this._authorizer.CanUnclaimSeat(actor, seat))
                    {
                        var errorCode = scope.Room.Status == DraftRoomStatus.Lobby
                            ? DraftRoomErrorCode.Forbidden
                            : DraftRoomErrorCode.RoomChanged;
                        return Task.FromResult(Failure(errorCode));
                    }

                    var participant = scope.Room.Participants.Single(item => item.Id == seat.ParticipantId);
                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.SeatUnclaimed,
                        actor.Participant.Id,
                        new
                        {
                            seat.Id,
                            seat.Index,
                            ParticipantId = participant.Id,
                            participant.DisplayName,
                        });
                    seat.ParticipantId = null;
                    seat.Participant = null;
                    seat.ClaimedAtUtc = null;

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
