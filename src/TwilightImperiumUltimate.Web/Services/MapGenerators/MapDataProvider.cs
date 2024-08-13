using TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapDataProvider : IMapDataProvider
{
    public IMapData GetMapData(MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.FivePlayersMediumWarpMap => new FivePlayersMediumWarpMapData(),
            MapTemplate.SixPlayersMediumMap => new SixPlayersMediumMapData(),
            MapTemplate.SixPlayersMediumSpiralMap => new SixPlayersMediumSpiralMapData(),
            _ => throw new NotImplementedException(),
        };
    }
}
