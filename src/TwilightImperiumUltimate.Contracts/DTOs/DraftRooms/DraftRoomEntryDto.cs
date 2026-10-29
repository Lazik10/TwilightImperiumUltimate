using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Identifies a newly created draft room and its host participant.
/// </summary>
public sealed class DraftRoomEntryDto
{
    /// <summary>
    /// Gets or sets the room identifier.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Gets or sets the public room code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the room display name.
    /// </summary>
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the host participant identifier.
    /// </summary>
    public Guid ParticipantId { get; set; }

    /// <summary>
    /// Gets or sets the host participant's display name.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the participant's room role.
    /// </summary>
    public DraftRoomRole Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the host is a guest.
    /// </summary>
    public bool IsGuest { get; set; }

    /// <summary>
    /// Gets or sets the guest secret, which is returned only when the host is a guest.
    /// </summary>
    public string? GuestSecret { get; set; }
}
