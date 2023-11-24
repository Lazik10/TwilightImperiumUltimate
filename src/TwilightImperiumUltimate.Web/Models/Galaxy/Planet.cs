using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.Galaxy;

public class Planet
{
    public PlanetName PlanetName { get; set; }

    public int Resources { get; set; }

    public int Influence { get; set; }

    public bool IsLegendary { get; set; }

    public TechnologyType TechnologySkip { get; set; }

    public PlanetTrait PlanetTrait { get; set; }

    public GameVersion GameVersion { get; set; }
}
