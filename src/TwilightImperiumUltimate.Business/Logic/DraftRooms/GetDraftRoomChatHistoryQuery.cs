using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Loads a page of authorized chat messages for a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="AfterId">An optional exclusive message cursor.</param>
/// <param name="Limit">The maximum number of messages to return.</param>
/// <param name="Caller">The account or guest identity requesting the history.</param>
public sealed record GetDraftRoomChatHistoryQuery(
    string Code,
    long? AfterId,
    int Limit,
    DraftRoomCaller Caller)
    : IRequest<Result<IReadOnlyList<ChatMessageDto>>>;
