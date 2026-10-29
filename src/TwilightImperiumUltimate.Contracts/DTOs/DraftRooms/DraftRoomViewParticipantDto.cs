using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes an active participant in a draft-room view.
/// </summary>
public sealed class DraftRoomViewParticipantDto
{
    /// <summary>
    /// Gets or sets the participant identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the participant's display name.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the participant's room role.
    /// </summary>
    public DraftRoomRole Role { get; set; }

    /// <summary>
    /// Gets or sets whether this participant is a guest.
    /// </summary>
    public bool IsGuest { get; set; }

    /// <summary>
    /// Gets or sets whether this participant is online in the room.
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// Gets or sets the participant's seat identifier, or <see langword="null"/> if unseated.
    /// </summary>
    public Guid? SeatId { get; set; }
}
