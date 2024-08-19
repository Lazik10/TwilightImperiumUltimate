namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersSmallMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersSmallMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 3, 7, 1 } },
        { 2, new List<int> { 18, 13, 14 } },
        { 3, new List<int> { 10, 11, 16 } },
    };
}
