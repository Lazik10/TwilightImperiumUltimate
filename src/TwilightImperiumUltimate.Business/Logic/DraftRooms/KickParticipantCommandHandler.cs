using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Kicks a participant and releases their seat when an authorized manager requests it.
/// </summary>
public sealed class KickParticipantCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog,
    IRoomChangeNotifier roomChangeNotifier)
    : IRequestHandler<KickParticipantCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    private readonly IRoomChangeNotifier _roomChangeNotifier = roomChangeNotifier
        ?? throw new ArgumentNullException(nameof(roomChangeNotifier));

    /// <inheritdoc />
    public async Task<Result> Handle(KickParticipantCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

        try
        {
            DraftRoomChatMessage? chatMessage = null;
            var displayName = string.Empty;
            var result = await this._unitOfWork.ExecuteAsync(
                request.Code,
                scope =>
                {
                    var actor = this._authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.NotFoundOrWrongPassword));
                    }

                    var target = scope.Room.Participants
                        .SingleOrDefault(participant => participant.Id == request.ParticipantId);
                    if (target is null)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.ParticipantNotFound));
                    }

                    if (!this._authorizer.CanKick(actor, target))
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.Forbidden));
                    }

                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.ParticipantKicked,
                        actor.Participant.Id,
                        new
                        {
                            ParticipantId = target.Id,
                            target.DisplayName,
                        });

                    var seat = scope.Room.Seats
                        .SingleOrDefault(item => item.ParticipantId == target.Id);
                    if (seat is not null)
                    {
                        this._draftRoomLog.Append(
                            scope,
                            DraftLogEntryType.SeatUnclaimed,
                            actor.Participant.Id,
                            new
                            {
                                seat.Id,
                                seat.Index,
                                ParticipantId = target.Id,
                                target.DisplayName,
                            });
                    }

                    target.Status = DraftParticipantStatus.Kicked;
                    target.LastSeenUtc = scope.Now;

                    displayName = target.DisplayName;
                    chatMessage = new DraftRoomChatMessage
                    {
                        RoomId = scope.Room.Id,
                        ParticipantId = target.Id,
                        Kind = ChatMessageKind.Leave,
                        Text = $"{target.DisplayName} left",
                        CreatedAtUtc = scope.Now,
                    };
                    scope.Context.DraftRoomChatMessages.Add(chatMessage);

                    if (seat is not null)
                    {
                        seat.ParticipantId = null;
                        seat.Participant = null;
                        seat.ClaimedAtUtc = null;
                    }

                    return Task.FromResult(Result.Ok());
                },
                cancellationToken).ConfigureAwait(false);
            if (result.IsSuccess && chatMessage is not null)
            {
                await this._roomChangeNotifier
                    .NotifyChatMessageReceivedAsync(chatMessage.RoomId, new ChatMessageDto
                    {
                        Id = chatMessage.Id,
                        ParticipantId = chatMessage.ParticipantId,
                        DisplayName = displayName,
                        Kind = chatMessage.Kind,
                        Text = chatMessage.Text,
                        CreatedAtUtc = chatMessage.CreatedAtUtc,
                    })
                    .ConfigureAwait(false);
            }

            return result;
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
