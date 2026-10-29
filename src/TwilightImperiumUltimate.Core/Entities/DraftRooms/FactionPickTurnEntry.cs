namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

/// <summary>
/// Stores a seat's position in the initial pick order, independent of its occupant.
/// </summary>
public sealed class FactionPickTurnEntry
{
    public Guid RoomId { get; set; }

    public int Position { get; set; }

    public Guid SeatId { get; set; }
}
