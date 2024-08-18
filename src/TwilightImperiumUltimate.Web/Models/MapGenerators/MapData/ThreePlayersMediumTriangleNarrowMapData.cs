namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersMediumTriangleNarrowMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumTriangleNarrowMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 19, 18, 11, 25, 17 } },
        { 2, new List<int> { 38, 39, 40, 31, 32 } },
        { 3, new List<int> { 15, 22, 28, 23, 30 } },
    };
}
