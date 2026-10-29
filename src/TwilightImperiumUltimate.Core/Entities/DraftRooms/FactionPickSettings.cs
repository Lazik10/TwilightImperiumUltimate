using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

/// <summary>
/// Stores a room's Faction Pick rules independently of its runtime state.
/// </summary>
public sealed class FactionPickSettings
{
    public Guid RoomId { get; set; }

    public List<GameVersion> GameVersions { get; set; } =
        [GameVersion.BaseGame, GameVersion.ProphecyOfKings, GameVersion.ThundersEdge];

    public FactionPickBansMode BansMode { get; set; }

    public int BansPerPlayer { get; set; } = 1;

    public int HostBanCount { get; set; }

    public FactionPickOrderMode PickOrderMode { get; set; }

    public List<Guid> SeatOrder { get; set; } = [];

    public int PicksPerPlayer { get; set; } = 1;

    public bool Snake { get; set; } = true;
}
