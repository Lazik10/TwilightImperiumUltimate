using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;

public partial class ThreePlayersMediumMantaRayMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsMediumMap);
}
