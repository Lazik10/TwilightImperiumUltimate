namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class EightPlayersLargeWarpMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.EightPlayersLargeWarpMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 5, 13, 3, 14, 22 } },
        { 2, new List<int> { 25, 24, 15, 33, 23 } },
        { 3, new List<int> { 53, 42, 34, 51, 41 } },
        { 4, new List<int> { 69, 60, 52, 59, 50 } },
        { 5, new List<int> { 66, 67, 68, 57, 58 } },
        { 6, new List<int> { 46, 56, 65, 47, 48 } },
        { 7, new List<int> { 27, 38, 37, 29, 30 } },
        { 8, new List<int> { 11, 20, 19, 12, 21 } },
    };
}
