using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Claims a free lobby seat for an active room participant.
/// </summary>
public sealed class ClaimSeatCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog)
    : IRequestHandler<ClaimSeatCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    /// <inheritdoc />
    public async Task<Result> Handle(ClaimSeatCommand request, CancellationToken cancellationToken)
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

                    if (!this._authorizer.CanClaimSeat(actor))
                    {
                        return Failure(DraftRoomErrorCode.RoomChanged);
                    }

                    var targetSeat = scope.Room.Seats.SingleOrDefault(seat => seat.Id == request.SeatId);
                    if (targetSeat is null)
                    {
                        return Failure(DraftRoomErrorCode.SeatNotFound);
                    }

                    if (actor.Seat?.Id == targetSeat.Id)
                    {
                        return Result.Ok();
                    }

                    if (targetSeat.ParticipantId is not null)
                    {
                        return Failure(DraftRoomErrorCode.SeatOccupied);
                    }

                    if (!this._authorizer.CanClaimSeat(actor, targetSeat))
                    {
                        return Failure(DraftRoomErrorCode.Forbidden);
                    }

                    if (actor.Seat is not null)
                    {
                        this._draftRoomLog.Append(
                            scope,
                            DraftLogEntryType.SeatUnclaimed,
                            actor.Participant.Id,
                            new
                            {
                                actor.Seat.Id,
                                actor.Seat.Index,
                                ParticipantId = actor.Participant.Id,
                                actor.Participant.DisplayName,
                            });
                        actor.Seat.ParticipantId = null;
                        actor.Seat.Participant = null;
                        actor.Seat.ClaimedAtUtc = null;

                        await scope.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    }

                    targetSeat.ParticipantId = actor.Participant.Id;
                    targetSeat.Participant = actor.Participant;
                    targetSeat.ClaimedAtUtc = scope.Now;
                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.SeatClaimed,
                        actor.Participant.Id,
                        new
                        {
                            targetSeat.Id,
                            targetSeat.Index,
                            ParticipantId = actor.Participant.Id,
                            actor.Participant.DisplayName,
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

    private static Result Failure(DraftRoomErrorCode code)
    {
        return Result.Fail(new DraftRoomError(code));
    }
}
