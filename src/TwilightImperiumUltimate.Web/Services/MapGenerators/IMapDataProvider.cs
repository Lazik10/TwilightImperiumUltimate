using TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapDataProvider
{
    IMapData GetMapData(MapTemplate mapTemplate);
}
