namespace TwilightImperiumUltimate.Web.Models.Galaxy;

public class SystemTileModel
{
    public int Id { get; set; }

    public SystemTileName SystemTileName { get; set; }

    public string SystemTileCode { get; set; } = string.Empty;

    public SystemTileCategory SystemTileCategory { get; set; }

    public int Resources { get; set; }

    public int Influence { get; set; }

    public bool HasPlanets { get; set; }

    public FactionName FactionName { get; set; }

    public IReadOnlyList<PlanetModel> Planets { get; set; } = new List<PlanetModel>();

    public AnomalyName AnomalyName { get; set; }

    public GameVersion GameVersion { get; set; }
}
