using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.PreviewMaps;

public partial class ThreePlayersSmallMapPreview
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = new Dictionary<int, SystemTileModel>();

    private IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsSmallMap);

    private SystemTileModel CurrentSystemTile { get; set; } = default!;
}
