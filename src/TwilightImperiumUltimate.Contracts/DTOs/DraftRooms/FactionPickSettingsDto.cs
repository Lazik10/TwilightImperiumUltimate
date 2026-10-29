using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Contracts.Serialization;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Contains the configurable rules of a Faction Pick draft.
/// </summary>
public sealed class FactionPickSettingsDto : DraftSettingsDto
{
    /// <summary>
    /// Gets or sets the enabled faction expansions without changing the existing GameVersion enum.
    /// </summary>
    [JsonConverter(typeof(GameVersionListJsonConverter))]
    public IReadOnlyList<GameVersion> GameVersions { get; set; } =
        [GameVersion.BaseGame, GameVersion.ProphecyOfKings, GameVersion.ThundersEdge];

    /// <summary>
    /// Gets or sets the bans mode.
    /// </summary>
    public FactionPickBansMode BansMode { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of simultaneous bans per seat.
    /// </summary>
    public int BansPerPlayer { get; set; } = 1;

    /// <summary>
    /// Gets or sets the maximum number of host bans.
    /// </summary>
    public int HostBanCount { get; set; }

    /// <summary>
    /// Gets or sets the initial seat-order rule.
    /// </summary>
    public FactionPickOrderMode PickOrderMode { get; set; }

    /// <summary>
    /// Gets or sets the configured lobby seat order, copied to runtime only when the draft starts.
    /// </summary>
    public IReadOnlyList<Guid> SeatOrder { get; set; } = [];

    /// <summary>
    /// Gets or sets the number of picks made by each seat.
    /// </summary>
    public int PicksPerPlayer { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether direction reverses between pick rounds.
    /// </summary>
    public bool Snake { get; set; } = true;
}
