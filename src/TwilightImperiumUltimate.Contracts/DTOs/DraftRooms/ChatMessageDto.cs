using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes a public message in a draft-room chat.
/// </summary>
public sealed class ChatMessageDto
{
    /// <summary>
    /// Gets or sets the persisted message identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the participant who sent or triggered the message, if any.
    /// </summary>
    public Guid? ParticipantId { get; set; }

    /// <summary>
    /// Gets or sets the participant's display name, if any.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the kind of message.
    /// </summary>
    public ChatMessageKind Kind { get; set; }

    /// <summary>
    /// Gets or sets the plain-text message content.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC time the message was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }
}
