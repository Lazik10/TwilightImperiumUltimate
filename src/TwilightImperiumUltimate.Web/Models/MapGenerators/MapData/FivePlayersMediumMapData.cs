namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class FivePlayersMediumMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.FivePlayersMediumMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 20, 19, 18, 11, 25 } },
        { 2, new List<int> { 40, 33, 34, 39, 32 } },
        { 3, new List<int> { 44, 38, 46, 37, 31 } },
        { 4, new List<int> { 28, 29, 36, 22, 30 } },
        { 5, new List<int> { 9, 16, 15, 14, 23 } },
    };
}
