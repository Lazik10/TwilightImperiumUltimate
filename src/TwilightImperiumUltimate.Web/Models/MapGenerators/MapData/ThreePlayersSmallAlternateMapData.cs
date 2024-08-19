namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersSmallAlternateMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersSmallAlternateMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 19, 13, 8, 9 } },
        { 2, new List<int> { 15, 11, 17, 21 } },
        { 3, new List<int> { 2, 7, 6, 5 } },
    };
}
