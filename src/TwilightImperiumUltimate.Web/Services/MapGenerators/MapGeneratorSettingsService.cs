using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorSettingsService : IMapGeneratorSettingsService
{
    public int MapScale { get; set; } = MapGeneratorOptions.MaxScale;

    public MapTemplate MapTemplate { get; set; } = MapGeneratorOptions.MapTemplate;

    public PlacementStyle PlacementStyle { get; set; } = MapGeneratorOptions.PlacementStyle;

    public SystemWeight SystemWeight { get; set; } = MapGeneratorOptions.SystemWeight;

    public GameVersion GameVersion { get; set; } = MapGeneratorOptions.GameVersion;

    public SystemTileOverlay SystemTileOverlay { get; set; } = MapGeneratorOptions.SystemTileOverlay;

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
}
