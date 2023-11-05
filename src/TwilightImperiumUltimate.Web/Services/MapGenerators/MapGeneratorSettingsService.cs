using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Settings.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorSettingsService : IMapGeneratorSettingsService
{
    public int MapScale { get; set; } = MapGeneratorSettings.MaxScale;

    public MapTemplate MapTemplate { get; set; } = MapGeneratorSettings.MapTemplate;

    public PlacementStyle PlacementStyle { get; set; } = MapGeneratorSettings.PlacementStyle;

    public SystemWeight SystemWeight { get; set; } = MapGeneratorSettings.SystemWeight;

    public GameVersion GameVersion { get; set; } = MapGeneratorSettings.GameVersion;

    public SystemTileOverlay SystemTileOverlay { get; set; } = MapGeneratorSettings.SystemTileOverlay;

    public void IncreaseMapScale()
    {
        if (MapScale <= MapGeneratorSettings.MaxScale - MapGeneratorSettings.ScaleIncrement)
            MapScale += MapGeneratorSettings.ScaleIncrement;
    }

    public void DecreaseMapScale()
    {
        if (MapScale >= MapGeneratorSettings.MinScale + MapGeneratorSettings.ScaleIncrement)
            MapScale -= MapGeneratorSettings.ScaleIncrement;
    }
}
