using TwilightImperiumUltimate.Web.Settings.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public partial class TriangleMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsTriangleMap);
}
