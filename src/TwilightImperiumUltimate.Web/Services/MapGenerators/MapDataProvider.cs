using TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapDataProvider : IMapDataProvider
{
    public IMapData GetMapData(MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.TwoPlayersMediumMap => new TwoPlayersMediumMapData(),
            MapTemplate.ThreePlayersSmallMap => new ThreePlayersSmallMapData(),
            MapTemplate.ThreePlayersSmallAlternateMap => new ThreePlayersSmallAlternateMapData(),
            MapTemplate.ThreePlayersMediumTriangleMap => new ThreePlayersMediumTriangleMapData(),
            MapTemplate.ThreePlayersMediumTriangleNarrowMap => new ThreePlayersMediumTriangleNarrowMapData(),
            MapTemplate.ThreePlayersMediumSnowflakeMap => new ThreePlayersMediumSnowflakeMapData(),
            MapTemplate.ThreePlayersMediumMantaRayMap => new ThreePlayersMediumMantaRayMapData(),
            MapTemplate.ThreePlayersMediumTridentMap => new ThreePlayersMediumTridentMapData(),
            MapTemplate.ThreePlayersMediumRexMap => new ThreePlayersMediumRexMapData(),
            MapTemplate.FourPlayersMediumMap => new FourPlayersMediumMapData(),
            MapTemplate.FourPlayersMediumHorizontalMap => new FourPlayersMediumHorizontalMapData(),
            MapTemplate.FourPlayersMediumVerticalMap => new FourPlayersMediumVerticalMapData(),
            MapTemplate.FourPlayersMediumGapsMap => new FourPlayersMediumGapsMapData(),
            MapTemplate.FourPlayersMediumWarpMap => new FourPlayersMediumWarpMapData(),
            MapTemplate.FourPlayersMediumMiniWarpMap => new FourPlayersMediumMiniWarpMapData(),
            MapTemplate.FourPlayersMediumDoubleWarpMap => new FourPlayersMediumDoubleWarpMapData(),
            MapTemplate.FivePlayersMediumMap => new FivePlayersMediumMapData(),
            MapTemplate.FivePlayersMediumHyperlineMap => new FivePlayersMediumWarpMapData(),
            MapTemplate.FivePlayersMediumDiamondMap => new FivePlayersMediumDiamondMapData(),
            MapTemplate.FivePlayersLargeFlatMap => new FivePlayersLargeFlatMapData(),
            MapTemplate.SixPlayersMediumMap => new SixPlayersMediumMapData(),
            MapTemplate.SixPlayersMediumSpiralMap => new SixPlayersMediumSpiralMapData(),
            MapTemplate.SixPlayersLargeMap => new SixPlayersLargeMapData(),
            MapTemplate.SevenPlayersLargeHyperlineMap => new SevenPlayersLargeHyperlineMapData(),
            MapTemplate.SevenPlayersLargeWarpMap => new SevenPlayersLargeWarpMapData(),
            MapTemplate.EightPlayersLargeMap => new EightPlayersLargeMapData(),
            MapTemplate.EightPlayersLargeWarpMap => new EightPlayersLargeWarpMapData(),
            _ => throw new NotImplementedException(),
        };
    }
}
