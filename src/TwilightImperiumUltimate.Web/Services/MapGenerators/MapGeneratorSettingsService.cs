using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorSettingsService : IMapGeneratorSettingsService
{
    public int MapScale { get; set; } = MapGeneratorOptions.MaxScale;

    public MapTemplate MapTemplate { get; set; } = MapGeneratorOptions.MapTemplate;

    public PlacementStyle PlacementStyle { get; set; } = MapGeneratorOptions.PlacementStyle;

    public SystemWeight SystemWeight { get; set; } = MapGeneratorOptions.SystemWeight;

    public List<GameVersion> GameVersions { get; set; } = new List<GameVersion>() { GameVersion.BaseGame, GameVersion.ProphecyOfKings };

    public SystemTileOverlay SystemTileOverlay { get; set; } = MapGeneratorOptions.SystemTileOverlay;

    public WormholeDensity WormholeDensity { get; set; } = MapGeneratorOptions.WormholeDensity;

    public int NumberOfLegendaryPlanets { get; set; } = MapGeneratorOptions.NumberOfLegendaryPlanets;

    public void IncreaseMapScale()
    {
        if (MapScale <= MapGeneratorOptions.MaxScale - MapGeneratorOptions.ScaleIncrement)
            MapScale += MapGeneratorOptions.ScaleIncrement;
    }

    public void DecreaseMapScale()
    {
        if (MapScale >= MapGeneratorOptions.MinScale + MapGeneratorOptions.ScaleIncrement)
            MapScale -= MapGeneratorOptions.ScaleIncrement;
    }

    public void UpdateGameVersion(GameVersion gameVersion)
    {
        if (!GameVersions.Remove(gameVersion))
            GameVersions.Add(gameVersion);
    }

    public void UpdateWormholeDensity(WormholeDensity wormholeDensity) => WormholeDensity = wormholeDensity;
}
