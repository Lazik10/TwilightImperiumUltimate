﻿using TwilightImperiumUltimate.Web.Settings.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;

public partial class SmallMapAlternateThreePlayers : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsSmallMap);
}
