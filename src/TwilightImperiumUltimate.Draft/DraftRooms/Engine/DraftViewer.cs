using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Identifies a verified viewer. Managing a room never grants access to another seat's private data.
/// </summary>
/// <param name="ParticipantId">The verified participant, or null for a public projection.</param>
/// <param name="SeatId">The viewer's currently occupied seat, or null when unseated.</param>
/// <param name="Role">The viewer's room role.</param>
public sealed record DraftViewer(Guid? ParticipantId, Guid? SeatId, DraftRoomRole Role)
{
    /// <summary>
    /// Determines whether a value can be included in this viewer's projection.
    /// </summary>
    public bool CanSee(DraftVisibility visibility, Guid? ownerSeatId = null)
    {
        return visibility switch
        {
            DraftVisibility.Public => true,
            DraftVisibility.SeatPrivate =>
                ParticipantId is Guid participantId
                && participantId != Guid.Empty
                && SeatId is Guid seatId
                && seatId != Guid.Empty
                && seatId == ownerSeatId,
            DraftVisibility.ServerOnly => false,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility)),
        };
    }
}
