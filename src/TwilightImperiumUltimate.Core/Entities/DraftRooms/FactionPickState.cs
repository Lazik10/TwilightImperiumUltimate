using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

/// <summary>
/// Stores the runtime phase and cursor; resetting a draft deletes this row.
/// </summary>
public sealed class FactionPickState
{
    public Guid RoomId { get; set; }

    public FactionPickPhase Phase { get; set; }

    public int TurnIndex { get; set; }

    public bool BansRevealed { get; set; }

    public List<Guid> LockedBanSeatIds { get; set; } = [];
}
