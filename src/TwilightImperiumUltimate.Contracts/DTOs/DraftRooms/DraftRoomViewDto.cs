using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Contains the safe, personalized view of a draft room.
/// </summary>
public sealed class DraftRoomViewDto
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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current room status.
    /// </summary>
    public DraftRoomStatus Status { get; set; }

    /// <summary>
    /// Gets or sets whether new participants are prevented from joining.
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets the room concurrency version.
    /// </summary>
    public int Version { get; set; }

    /// <summary>Gets or sets the selected draft engine.</summary>
    public DraftTypeId DraftType { get; set; }

    /// <summary>Gets or sets the shared rules, editable only in the lobby by managers.</summary>
    public DraftSettingsDto Settings { get; set; } = new FactionPickSettingsDto();

    /// <summary>Gets or sets the personalized projection, absent in the lobby.</summary>
    public DraftViewDto? Draft { get; set; }

    /// <summary>Gets or sets whether the requesting participant can start now.</summary>
    public bool CanStart { get; set; }

    /// <summary>Gets or sets the lifecycle reasons preventing Start.</summary>
    public IReadOnlyList<string> StartBlockers { get; set; } = [];

    /// <summary>Gets or sets whether the requesting participant can reset the running draft.</summary>
    public bool CanReset { get; set; }

    /// <summary>
    /// Gets or sets the active participants in the room.
    /// </summary>
    public IReadOnlyList<DraftRoomViewParticipantDto> Participants { get; set; }
        = Array.Empty<DraftRoomViewParticipantDto>();

    /// <summary>
    /// Gets or sets the room's seats.
    /// </summary>
    public IReadOnlyList<DraftRoomViewSeatDto> Seats { get; set; }
        = Array.Empty<DraftRoomViewSeatDto>();

    /// <summary>
    /// Gets or sets the requesting participant and their room permissions.
    /// </summary>
    public DraftRoomViewMeDto Me { get; set; } = new();
}
