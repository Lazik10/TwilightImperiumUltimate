namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class FourPlayersMediumDoubleWarpMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumDoubleWarpMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 27, 19, 12, 18, 25 } },
        { 2, new List<int> { 40, 33, 34, 39, 32 } },
        { 3, new List<int> { 28, 29, 36, 37, 30 } },
        { 4, new List<int> { 8, 15, 21, 16, 23 } },
    };
}
