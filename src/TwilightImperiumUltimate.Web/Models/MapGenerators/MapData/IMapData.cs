namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public interface IMapData
{
    MapTemplate MapTemplate { get; }

    Dictionary<int, List<int>> SlicePositions { get; }
}
