using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Sends a public text message to a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="Text">The plain-text message content.</param>
/// <param name="Caller">The account or guest identity sending the message.</param>
public sealed record SendChatCommand(string Code, string? Text, DraftRoomCaller Caller)
    : IRequest<Result<ChatMessageDto>>;
