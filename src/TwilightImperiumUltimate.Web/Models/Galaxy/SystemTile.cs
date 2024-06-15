using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.Galaxy;

public class SystemTile
{
    public int Id { get; set; }

    public SystemTileName Name { get; set; }

    public SystemTileCategory TileCategory { get; set; }

    public int Resources { get; set; }

    public int Influence { get; set; }

    public bool HasPlanets { get; set; }

    public FactionName FactionName { get; set; }

    public IReadOnlyList<Planet> Planets { get; set; } = new List<Planet>();
}
