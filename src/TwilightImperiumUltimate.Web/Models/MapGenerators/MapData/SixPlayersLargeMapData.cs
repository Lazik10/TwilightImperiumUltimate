namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class SixPlayersLargeMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.SixPlayersLargeMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 5, 13, 3, 14, 12 } },
        { 2, new List<int> { 35, 25, 16, 34, 24 } },
        { 3, new List<int> { 61, 52, 53, 60, 43 } },
        { 4, new List<int> { 66, 67, 68, 57, 59 } },
        { 5, new List<int> { 45, 46, 55, 37, 56 } },
        { 6, new List<int> { 10, 19, 27, 20, 28 } },
    };
}
