namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class SixPlayersMediumMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.SixPlayersMediumMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 9, 10, 11, 18, 17 } },
        { 2, new List<int> { 12, 19, 27, 26, 25 } },
        { 3, new List<int> { 40, 33, 34, 39, 32 } },
        { 4, new List<int> { 44, 38, 46, 37, 31 } },
        { 5, new List<int> { 28, 29, 36, 22, 30 } },
        { 6, new List<int> { 8, 15, 21, 16, 23 } },
    };
}
