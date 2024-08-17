using TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapDataProvider : IMapDataProvider
{
    public IMapData GetMapData(MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.FourPlayersMediumMap => new FourPlayersMediumMapData(),
            MapTemplate.FourPlayersMediumHorizontalMap => new FourPlayersMediumHorizontalMapData(),
            MapTemplate.FourPlayersMediumVerticalMap => new FourPlayersMediumVerticalMapData(),
            MapTemplate.FourPlayersMediumGapsMap => new FourPlayersMediumGapsMapData(),
            MapTemplate.FourPlayersMediumWarpMap => new FourPlayersMediumWarpMapData(),
            MapTemplate.FourPlayersMediumMiniWarpMap => new FourPlayersMediumMiniWarpMapData(),
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
