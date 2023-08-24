using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.Factions;

public class Faction
{
    public FactionName FactionName { get; set; }

    public int Commodities { get; set; }

    public int ComplexityRating { get; set; }

    public GameVersion GameVersion { get; set; }
}
