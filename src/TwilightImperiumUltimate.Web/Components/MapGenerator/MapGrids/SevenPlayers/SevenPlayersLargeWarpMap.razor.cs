using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SevenPlayers;

public partial class SevenPlayersLargeWarpMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsLargeMap);
}