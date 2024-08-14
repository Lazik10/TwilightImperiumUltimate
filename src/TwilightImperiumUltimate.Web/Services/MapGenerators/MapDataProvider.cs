using TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapDataProvider : IMapDataProvider
{
    public IMapData GetMapData(MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.FivePlayersMediumHyperlineMap => new FivePlayersMediumWarpMapData(),
            MapTemplate.SixPlayersMediumMap => new SixPlayersMediumMapData(),
            MapTemplate.SixPlayersMediumSpiralMap => new SixPlayersMediumSpiralMapData(),
            MapTemplate.SixPlayersLargeMap => new SixPlayersLargeMapData(),
            MapTemplate.EightPlayersLargeMap => new EightPlayersLargeMapData(),
            MapTemplate.EightPlayersLargeWarpMap => new EightPlayersLargeWarpMapData(),
            _ => throw new NotImplementedException(),
        };
    }
}
