using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes the requesting participant and their effective room permissions.
/// </summary>
public sealed class DraftRoomViewMeDto
{
    /// <summary>
    /// Gets or sets the requesting participant identifier.
    /// </summary>
    public Guid ParticipantId { get; set; }

    /// <summary>
    /// Gets or sets the requesting participant's room role.
    /// </summary>
    public DraftRoomRole Role { get; set; }

    /// <summary>
    /// Gets or sets the requesting participant's seat identifier, or <see langword="null"/> if unseated.
    /// </summary>
    public Guid? SeatId { get; set; }

    /// <summary>
    /// Gets or sets whether the participant can add or remove seats.
    /// </summary>
    public bool CanManageSeats { get; set; }

    /// <summary>
    /// Gets or sets whether the participant can change room settings.
    /// </summary>
    public bool CanChangeSettings { get; set; }

    /// <summary>
    /// Gets or sets whether the participant can kick at least one active room participant.
    /// </summary>
    public bool CanKick { get; set; }

    /// <summary>
    /// Gets or sets whether the participant can change at least one active participant's role.
    /// </summary>
    public bool CanChangeRoles { get; set; }

    /// <summary>
    /// Gets or sets whether the participant can close the room.
    /// </summary>
    public bool CanClose { get; set; }

    /// <summary>
    /// Gets or sets whether the participant can claim or move their own seat.
    /// </summary>
    public bool CanClaimSeat { get; set; }
}
