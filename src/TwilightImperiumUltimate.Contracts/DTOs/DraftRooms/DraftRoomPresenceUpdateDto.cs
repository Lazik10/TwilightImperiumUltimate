namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes a participant presence transition sent to room subscribers.
/// </summary>
public sealed class DraftRoomPresenceUpdateDto
{
    /// <summary>
    /// Gets or sets the participant identifier.
    /// </summary>
    public Guid ParticipantId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the participant is online.
    /// </summary>
    public bool IsOnline { get; set; }
}
