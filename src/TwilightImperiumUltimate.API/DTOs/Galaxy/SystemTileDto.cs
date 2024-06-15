using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.Core.Enums.Races;

namespace TwilightImperiumUltimate.API.DTOs.Galaxy;

public class SystemTileDto
{
    public int Id { get; set; }

    public SystemTileName Name { get; set; }

    public SystemTileCategory TileCategory { get; set; }

    public int Resources { get; set; }

    public int Influence { get; set; }

    public bool HasPlanets { get; set; }

    public FactionName FactionName { get; set; }

    public IReadOnlyList<PlanetDto> Planets { get; set; } = new List<PlanetDto>();
}
