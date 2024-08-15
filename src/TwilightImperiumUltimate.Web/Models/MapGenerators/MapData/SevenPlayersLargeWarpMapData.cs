namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class SevenPlayersLargeWarpMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.SevenPlayersLargeWarpMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 5, 13, 3, 23, 22 } },
        { 2, new List<int> { 34, 33, 24, 42, 32 } },
        { 3, new List<int> { 60, 51, 43, 50, 41 } },
        { 4, new List<int> { 66, 67, 68, 57, 58 } },
        { 5, new List<int> { 46, 56, 65, 47, 48 } },
        { 6, new List<int> { 27, 38, 37, 29, 30 } },
        { 7, new List<int> { 11, 20, 19, 12, 21 } },
    };
}
