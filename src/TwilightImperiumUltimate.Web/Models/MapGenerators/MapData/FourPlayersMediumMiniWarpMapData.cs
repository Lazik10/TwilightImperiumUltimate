namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class FourPlayersMediumMiniWarpMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumMiniWarpMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 34, 26, 19, 20, 25 } },
        { 2, new List<int> { 45, 38, 39, 40, 31 } },
        { 3, new List<int> { 21, 22, 29, 35, 30 } },
        { 4, new List<int> { 3, 10, 16, 9, 17 } },
    };
}
