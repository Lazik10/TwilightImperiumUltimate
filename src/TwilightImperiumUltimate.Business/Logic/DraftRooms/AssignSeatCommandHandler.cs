using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Performs a manager's seat assignment under the same eligibility policy as self-claim.</summary>
public sealed class AssignSeatCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog log) : IRequestHandler<AssignSeatCommand, Result>
{
    private readonly IDraftRoomUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IDraftRoomAuthorizer authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));
    private readonly IDraftRoomLog log = log ?? throw new ArgumentNullException(nameof(log));

    /// <inheritdoc />
    public async Task<Result> Handle(AssignSeatCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        try
        {
            return await this.unitOfWork.ExecuteAsync(
                request.Code,
                scope =>
                {
                    var actor = this.authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Task.FromResult(Result.Fail(new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword)));
                    }

                    if (!this.authorizer.CanManageDraft(actor) || scope.Room.Status == DraftRoomStatus.Finished)
                    {
                        return Task.FromResult(Result.Fail(new DraftRoomError(DraftRoomErrorCode.Forbidden)));
                    }

                    var seat = scope.Room.Seats.SingleOrDefault(item => item.Id == request.SeatId);
                    var participant = scope.Room.Participants.SingleOrDefault(item =>
                        item.Id == request.ParticipantId && item.Status == DraftParticipantStatus.Active);
                    if (seat is null || participant is null)
                    {
                        return Task.FromResult(Result.Fail(new DraftRoomError(
                            seat is null ? DraftRoomErrorCode.SeatNotFound : DraftRoomErrorCode.ParticipantNotFound)));
                    }

                    var targetActor = new DraftRoomActor(
                        scope.Room, participant, scope.Room.Seats.SingleOrDefault(item => item.ParticipantId == participant.Id));
                    if (targetActor.Seat is not null || !this.authorizer.CanClaimSeat(targetActor, seat))
                    {
                        return Task.FromResult(Result.Fail(new DraftRoomError(DraftRoomErrorCode.Forbidden)));
                    }

                    seat.ParticipantId = participant.Id;
                    seat.Participant = participant;
                    seat.ClaimedAtUtc = scope.Now;
                    this.log.Append(
                        scope,
                        DraftLogEntryType.SeatClaimed,
                        actor.Participant.Id,
                        new { seat.Id, seat.Index, ParticipantId = participant.Id, participant.DisplayName });
                    return Task.FromResult(Result.Ok());
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
