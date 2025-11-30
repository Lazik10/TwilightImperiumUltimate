using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;

public class RemovePrestigeRankResponse
{
    public bool Success { get; set; }
    public string ErrorTitle { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public int TiglUserId { get; set; }
    public TiglPrestigeRank PrestigeRank { get; set; }
    public TiglLeague League { get; set; }
    public TiglFactionName Faction { get; set; }
    public int Level { get; set; }
}
