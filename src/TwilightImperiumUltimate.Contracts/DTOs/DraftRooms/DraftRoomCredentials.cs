namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Supplies guest credentials when subscribing to a draft-room hub.
/// </summary>
public sealed class DraftRoomCredentials
{
    /// <summary>
    /// Gets or sets the guest participant identifier.
    /// </summary>
    public Guid? GuestParticipantId { get; set; }

    /// <summary>
    /// Gets or sets the guest secret, sent in the hub message rather than the connection URL.
    /// </summary>
    public string? GuestSecret { get; set; }
}
