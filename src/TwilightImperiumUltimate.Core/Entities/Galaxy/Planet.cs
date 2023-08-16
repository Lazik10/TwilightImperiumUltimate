using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Technologies;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Galaxy;

public class Planet : IEntity, IGameVersion
{
    public int Id { get; set; }

    public PlanetName PlanetName { get; set; }

    public int Resources { get; set; }

    public int Influence { get; set; }

    public bool IsLegendary { get; set; }

    public TechnologyType TechnologySkip { get; set; }

    public PlanetTrait PlanetTrait { get; set; }

    public SystemTileName SystemTileName { get; set; }

    public SystemTile SystemTile { get; set; } = default!;

    public GameVersion GameVersion { get; set; }
}
