using FluentResults;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Persists a participant's chat message and publishes it after commit.
/// </summary>
public sealed class SendChatCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomChatThrottle chatThrottle,
    IRoomChangeNotifier roomChangeNotifier,
    ILogger<SendChatCommandHandler> logger)
    : IRequestHandler<SendChatCommand, Result<ChatMessageDto>>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomChatThrottle _chatThrottle = chatThrottle
        ?? throw new ArgumentNullException(nameof(chatThrottle));

    private readonly IRoomChangeNotifier _roomChangeNotifier = roomChangeNotifier
        ?? throw new ArgumentNullException(nameof(roomChangeNotifier));

    private readonly ILogger<SendChatCommandHandler> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<Result<ChatMessageDto>> Handle(
        SendChatCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

        var text = request.Text?.Trim();
        if (string.IsNullOrWhiteSpace(text) || text.Length > DraftRoomLimits.MaxChatMessageLength)
        {
            return Failure(DraftRoomErrorCode.InvalidChatMessage);
        }

        try
        {
            var operationResult = await this._unitOfWork.ExecuteAsync(
                request.Code,
                scope =>
                {
                    var actor = this._authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Task.FromResult(OperationFailure(
                            DraftRoomErrorCode.NotFoundOrWrongPassword));
                    }

                    if (!this._chatThrottle.TryAcquire(actor.Participant.Id))
                    {
                        return Task.FromResult(OperationFailure(DraftRoomErrorCode.ChatRateLimited));
                    }

                    var message = new DraftRoomChatMessage
                    {
                        RoomId = scope.Room.Id,
                        ParticipantId = actor.Participant.Id,
                        Kind = ChatMessageKind.User,
                        Text = text,
                        CreatedAtUtc = scope.Now,
                    };
                    scope.Context.DraftRoomChatMessages.Add(message);

                    return Task.FromResult(Result.Ok((
                        Message: message,
                        DisplayName: actor.Participant.DisplayName,
                        RoomId: scope.Room.Id)));
                },
                cancellationToken,
                changesState: false).ConfigureAwait(false);

            if (operationResult.IsFailed)
            {
                return Result.Fail<ChatMessageDto>(operationResult.Errors);
            }

            var pendingMessage = operationResult.Value;
            var dto = new ChatMessageDto
            {
                Id = pendingMessage.Message.Id,
                ParticipantId = pendingMessage.Message.ParticipantId,
                DisplayName = pendingMessage.DisplayName,
                Kind = pendingMessage.Message.Kind,
                Text = pendingMessage.Message.Text,
                CreatedAtUtc = pendingMessage.Message.CreatedAtUtc,
            };

            await this._roomChangeNotifier
                .NotifyChatMessageReceivedAsync(pendingMessage.RoomId, dto)
                .ConfigureAwait(false);

            return Result.Ok(dto);
        }
        catch (DraftRoomNotFoundException exception)
        {
            this._logger.LogInformation(
                exception,
                "DraftRoomChatRejected Code={Code} Reason={Reason}",
                request.Code,
                "NotFound");

            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }
        catch (DraftRoomConflictException)
        {
            return Failure(DraftRoomErrorCode.RoomChanged);
        }
    }

    private static Result<ChatMessageDto> Failure(DraftRoomErrorCode code)
    {
        return Result.Fail<ChatMessageDto>(new DraftRoomError(code));
    }

    private static Result<(DraftRoomChatMessage Message, string DisplayName, Guid RoomId)> OperationFailure(
        DraftRoomErrorCode code)
    {
        return Result.Fail<(DraftRoomChatMessage Message, string DisplayName, Guid RoomId)>(
            new DraftRoomError(code));
    }
}
