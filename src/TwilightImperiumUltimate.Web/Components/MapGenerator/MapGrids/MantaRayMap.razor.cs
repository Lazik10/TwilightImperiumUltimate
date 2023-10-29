using TwilightImperiumUltimate.Web.Settings.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public partial class MantaRayMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsMantaRayMap);
}
