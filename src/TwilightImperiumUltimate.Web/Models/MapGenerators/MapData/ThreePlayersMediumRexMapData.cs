namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersMediumRexMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumRexMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 18, 16, 3, 19, 25 } },
        { 2, new List<int> { 39, 26, 41, 38, 31 } },
        { 3, new List<int> { 22, 37, 35, 15, 23 } },
    };
}
