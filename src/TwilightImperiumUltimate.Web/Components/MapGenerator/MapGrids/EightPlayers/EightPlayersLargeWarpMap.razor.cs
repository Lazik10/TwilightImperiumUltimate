using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.EightPlayers;

public partial class EightPlayersLargeWarpMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsLargeMap);
}