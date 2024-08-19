namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersMediumTridentMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumTridentMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 11, 10, 9, 19, 17 } },
        { 2, new List<int> { 40, 33, 34, 38, 32 } },
        { 3, new List<int> { 28, 29, 36, 15, 30 } },
    };
}
