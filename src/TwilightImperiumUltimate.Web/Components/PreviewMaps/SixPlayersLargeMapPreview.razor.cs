using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.PreviewMaps;

public partial class SixPlayersLargeMapPreview
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = new Dictionary<int, SystemTileModel>();

    private IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsLargeMap);

    private SystemTileModel CurrentSystemTile { get; set; } = default!;
}
