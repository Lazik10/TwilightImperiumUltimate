using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;

public class RemovePrestigeRankRequest
{
    public int TiglUserId { get; set; }

    public TiglPrestigeRank PrestigeRank { get; set; }

    public TiglLeague League { get; set; }

    public TiglFactionName Faction { get; set; } = TiglFactionName.None;

    public int Level { get; set; }
}
