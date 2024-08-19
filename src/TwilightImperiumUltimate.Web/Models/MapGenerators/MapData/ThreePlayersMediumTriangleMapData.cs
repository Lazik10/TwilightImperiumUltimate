namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersMediumTriangleMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumTriangleMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 11, 10, 9, 18, 16 } },
        { 2, new List<int> { 40, 33, 34, 39, 26 } },
        { 3, new List<int> { 28, 29, 36, 22, 37 } },
    };
}
