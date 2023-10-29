﻿using TwilightImperiumUltimate.Web.Settings.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public partial class TriangleNarrowMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsTriangleNarrowMap);
}
