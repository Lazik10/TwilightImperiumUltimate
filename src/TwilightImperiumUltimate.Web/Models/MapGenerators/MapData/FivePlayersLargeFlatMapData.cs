namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class FivePlayersLargeFlatMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.FivePlayersLargeFlatMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 35, 34, 33, 42, 32 } },
        { 2, new List<int> { 60, 51, 43, 53, 41 } },
        { 3, new List<int> { 57, 58, 59, 48, 50 } },
        { 4, new List<int> { 45, 37, 47, 56, 39 } },
        { 5, new List<int> { 29, 28, 27, 38, 30 } },
    };
}
