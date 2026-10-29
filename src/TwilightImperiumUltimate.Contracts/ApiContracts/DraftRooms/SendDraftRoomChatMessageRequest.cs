namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>
/// Request to send a text message to a draft room.
/// </summary>
public sealed record SendDraftRoomChatMessageRequest
{
    /// <summary>
    /// Gets or sets the plain-text message content.
    /// </summary>
    public string? Text { get; init; }
}
