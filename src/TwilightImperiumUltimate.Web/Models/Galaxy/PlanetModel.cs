using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.Galaxy;

public class PlanetModel
{
    public PlanetName PlanetName { get; set; }

    public int Resources { get; set; }

    public int Influence { get; set; }

    public bool IsLegendary { get; set; }

    public TechnologyType TechnologySkip { get; set; }

    public PlanetTrait PlanetTrait { get; set; }

    public GameVersion GameVersion { get; set; }
}
