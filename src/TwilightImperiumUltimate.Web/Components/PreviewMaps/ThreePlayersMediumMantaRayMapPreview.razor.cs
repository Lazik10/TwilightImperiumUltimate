using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.PreviewMaps;

public partial class ThreePlayersMediumMantaRayMapPreview
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = new Dictionary<int, SystemTileModel>();

    private IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsMediumMap);

    private SystemTileModel CurrentSystemTile { get; set; } = default!;
}
