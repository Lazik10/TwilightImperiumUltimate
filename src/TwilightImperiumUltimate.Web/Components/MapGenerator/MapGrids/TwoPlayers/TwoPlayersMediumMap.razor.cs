using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.TwoPlayers;

public partial class TwoPlayersMediumMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsMediumMap);
}