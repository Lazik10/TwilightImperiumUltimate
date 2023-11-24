using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Technologies;

namespace TwilightImperiumUltimate.API.DTOs.Galaxy;

public class PlanetDto
{
    public PlanetName PlanetName { get; set; }

    public int Resources { get; set; }

    public int Influence { get; set; }

    public bool IsLegendary { get; set; }

    public TechnologyType TechnologySkip { get; set; }

    public PlanetTrait PlanetTrait { get; set; }

    public GameVersion GameVersion { get; set; }
}
