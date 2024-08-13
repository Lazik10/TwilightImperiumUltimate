namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class SixPlayersMediumSpiralMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.SixPlayersMediumSpiralMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 12, 18, 10, 3, 17 } },
        { 2, new List<int> { 34, 26, 19, 20, 25 } },
        { 3, new List<int> { 46, 39, 33, 41, 32 } },
        { 4, new List<int> { 36, 37, 38, 45, 31 } },
        { 5, new List<int> { 21, 22, 29, 35, 30 } },
        { 6, new List<int> { 9, 16, 15, 14, 23 } },
    };
}
