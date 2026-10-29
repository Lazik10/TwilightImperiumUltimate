using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Marks the caller as left and releases their seat while the room is in its lobby.
/// </summary>
public sealed class LeaveRoomCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog,
    IRoomChangeNotifier roomChangeNotifier)
    : IRequestHandler<LeaveRoomCommand, Result>
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
    public async Task<Result> Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
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

                    if (scope.Room.Status != DraftRoomStatus.Lobby)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.RoomChanged));
                    }

                    this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.ParticipantLeft,
                        actor.Participant.Id,
                        new { actor.Participant.Id, actor.Participant.DisplayName });

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
                    }

                    actor.Participant.Status = DraftParticipantStatus.Left;
                    actor.Participant.LastSeenUtc = scope.Now;

                    displayName = actor.Participant.DisplayName;
                    chatMessage = new DraftRoomChatMessage
                    {
                        RoomId = scope.Room.Id,
                        ParticipantId = actor.Participant.Id,
                        Kind = ChatMessageKind.Leave,
                        Text = $"{actor.Participant.DisplayName} left",
                        CreatedAtUtc = scope.Now,
                    };
                    scope.Context.DraftRoomChatMessages.Add(chatMessage);

                    if (actor.Seat is not null)
                    {
                        actor.Seat.ParticipantId = null;
                        actor.Seat.Participant = null;
                        actor.Seat.ClaimedAtUtc = null;
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
