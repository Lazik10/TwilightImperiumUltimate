using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Contains the public phase of a Faction Pick draft.
/// Seat-private selections are added only by the engine's viewer projection.
/// </summary>
public sealed class FactionPickViewDto : DraftViewDto
{
    /// <summary>
    /// Gets or sets the draft's current phase.
    /// </summary>
    public FactionPickPhase Phase { get; set; }

    /// <summary>
    /// Gets or sets the public initial pick order by seat identifier.
    /// </summary>
    public IReadOnlyList<Guid> TurnOrder { get; set; } = [];

    /// <summary>Gets or sets all factions available in the configured expansions.</summary>
    public IReadOnlyList<FactionName> Pool { get; set; } = [];

    /// <summary>Gets or sets publicly visible host bans or the simultaneously revealed bans.</summary>
    public IReadOnlyList<FactionName> PublicBans { get; set; } = [];

    /// <summary>Gets or sets only the verified viewer's own ban choices.</summary>
    public IReadOnlyList<FactionName> MyBans { get; set; } = [];

    /// <summary>Gets or sets the public lock flags, without unrevealed factions.</summary>
    public IReadOnlyList<Guid> LockedBanSeatIds { get; set; } = [];

    public bool BansRevealed { get; set; }

    public int TurnIndex { get; set; }

    public Guid? ActiveSeatId { get; set; }

    public FactionName? MyPendingPick { get; set; }

    public IReadOnlyList<FactionPickLockedPickDto> LockedPicks { get; set; } = [];

    public bool CanUndo { get; set; }
}
