namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class FivePlayersMediumMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.FivePlayersMediumMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 19, 18, 11, 25 } },
        { 2, new List<int> { 33, 26, 27, 32 } },
        { 3, new List<int> { 44, 38, 46, 31 } },
        { 4, new List<int> { 21, 22, 29, 30 } },
        { 5, new List<int> { 9, 16, 15, 23 } },
    };
}
