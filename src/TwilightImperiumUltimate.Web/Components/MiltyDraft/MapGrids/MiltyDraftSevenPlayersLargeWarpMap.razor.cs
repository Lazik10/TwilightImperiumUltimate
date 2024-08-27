using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft.MapGrids;

public partial class MiltyDraftSevenPlayersLargeWarpMap
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = new Dictionary<int, SystemTileModel>();

    private IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsLargeMap);

    private SystemTileModel CurrentSystemTile { get; set; } = default!;
}