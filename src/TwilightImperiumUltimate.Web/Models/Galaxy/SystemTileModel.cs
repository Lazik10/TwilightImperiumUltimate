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

    public string PlayerName { get; set; } = string.Empty;

    public IReadOnlyList<PlanetModel> Planets { get; set; } = new List<PlanetModel>();

    public IReadOnlyCollection<WormholeModel> Wormholes { get; set; } = new List<WormholeModel>();

    public AnomalyName AnomalyName { get; set; }

    public GameVersion GameVersion { get; set; }

    public SystemTileModel Copy()
    {
        return new SystemTileModel
        {
            Id = Id,
            SystemTileName = SystemTileName,
            SystemTileCode = SystemTileCode,
            SystemTileCategory = SystemTileCategory,
            Resources = Resources,
            Influence = Influence,
            HasPlanets = HasPlanets,
            FactionName = FactionName,
            PlayerName = PlayerName,
            Planets = new List<PlanetModel>(Planets),
            AnomalyName = AnomalyName,
            GameVersion = GameVersion,
        };
    }
}
