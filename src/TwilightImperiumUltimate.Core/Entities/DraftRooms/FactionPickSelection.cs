using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

/// <summary>Stores a seat's private pending or locked faction choice. Host bans have no seat.</summary>
public sealed class FactionPickSelection
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public Guid? SeatId { get; set; }

    public FactionPickSelectionKind Kind { get; set; }

    public FactionName Faction { get; set; }

    public FactionPickSelectionState State { get; set; }

    public int Round { get; set; }

    public Guid? DecidedByParticipantId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? LockedAtUtc { get; set; }
}
