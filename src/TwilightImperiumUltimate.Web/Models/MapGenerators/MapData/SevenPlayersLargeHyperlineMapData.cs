namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class SevenPlayersLargeHyperlineMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.SevenPlayersLargeHyperlineMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 5, 13, 3, 14, 12 } },
        { 2, new List<int> { 26, 25, 24, 15, 23 } },
        { 3, new List<int> { 53, 43, 34, 35, 42 } },
        { 4, new List<int> { 69, 60, 52, 62, 50 } },
        { 5, new List<int> { 54, 46, 56, 65, 48 } },
        { 6, new List<int> { 27, 28, 37, 45, 38 } },
        { 7, new List<int> { 11, 20, 19, 18, 21 } },
    };
}
