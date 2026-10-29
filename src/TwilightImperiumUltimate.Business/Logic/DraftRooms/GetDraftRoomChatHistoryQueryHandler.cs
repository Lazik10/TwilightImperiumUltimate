using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Loads a bounded page of chat messages for an active room participant.
/// </summary>
public sealed class GetDraftRoomChatHistoryQueryHandler(
    IDraftRoomRepository draftRoomRepository,
    IDraftRoomAuthorizer authorizer)
    : IRequestHandler<GetDraftRoomChatHistoryQuery, Result<IReadOnlyList<ChatMessageDto>>>
{
    private readonly IDraftRoomRepository _draftRoomRepository = draftRoomRepository
        ?? throw new ArgumentNullException(nameof(draftRoomRepository));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<ChatMessageDto>>> Handle(
        GetDraftRoomChatHistoryQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Caller);

        if (request.AfterId is < 0)
        {
            return Failure(DraftRoomErrorCode.InvalidHistoryCursor);
        }

        if (request.Limit is < 1 or > DraftRoomLimits.MaxHistoryPageSize)
        {
            return Failure(DraftRoomErrorCode.InvalidHistoryLimit);
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var room = await this._draftRoomRepository
            .GetRoomByCodeAsync(request.Code.Trim().ToUpperInvariant(), cancellationToken)
            .ConfigureAwait(false);
        if (room is null || this._authorizer.Resolve(room, request.Caller) is null)
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var messages = await this._draftRoomRepository
            .GetChatHistoryAsync(room.Id, request.AfterId, request.Limit, cancellationToken)
            .ConfigureAwait(false);
        var displayNamesByParticipantId = room.Participants
            .ToDictionary(participant => participant.Id, participant => participant.DisplayName);

        return Result.Ok<IReadOnlyList<ChatMessageDto>>(messages
            .Select(message => new ChatMessageDto
            {
                Id = message.Id,
                ParticipantId = message.ParticipantId,
                DisplayName = message.ParticipantId is Guid participantId
                    && displayNamesByParticipantId.TryGetValue(participantId, out var displayName)
                        ? displayName
                        : null,
                Kind = message.Kind,
                Text = message.Text,
                CreatedAtUtc = message.CreatedAtUtc,
            })
            .ToArray());
    }

    private static Result<IReadOnlyList<ChatMessageDto>> Failure(DraftRoomErrorCode code)
    {
        return Result.Fail<IReadOnlyList<ChatMessageDto>>(new DraftRoomError(code));
    }
}
