using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes a draft room in an account's room list.
/// </summary>
public sealed class MyDraftRoomDto
{
    /// <summary>
    /// Gets or sets the room's public code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the room's display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the room's current status.
    /// </summary>
    public DraftRoomStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the account participant's room role.
    /// </summary>
    public DraftRoomRole Role { get; set; }

    /// <summary>
    /// Gets or sets the account participant's seat index, or <see langword="null"/> when unseated.
    /// </summary>
    public int? SeatIndex { get; set; }

    /// <summary>
    /// Gets or sets the time of the room's latest activity in UTC.
    /// </summary>
    public DateTime LastActivityUtc { get; set; }
}
