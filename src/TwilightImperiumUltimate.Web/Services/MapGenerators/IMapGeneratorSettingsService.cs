namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorSettingsService
{
    public int MapScale { get; set; }

    public MapTemplate MapTemplate { get; set; }

    public PlacementStyle PlacementStyle { get; set; }

    public SystemWeight SystemWeight { get; set; }

    public List<GameVersion> GameVersions { get; set; }

    public SystemTileOverlay SystemTileOverlay { get; set; }

    public WormholeDensity WormholeDensity { get; set; }

    public int NumberOfLegendaryPlanets { get; set; }

    public void IncreaseMapScale();

    public void DecreaseMapScale();

    public void UpdateGameVersion(GameVersion gameVersion);

    public void UpdateWormholeDensity(WormholeDensity wormholeDensity);
}
