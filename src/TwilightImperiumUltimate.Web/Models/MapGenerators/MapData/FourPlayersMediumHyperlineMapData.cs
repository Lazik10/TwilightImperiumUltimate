namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class FourPlayersMediumHyperlineMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumHyperlineMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 27, 19, 12, 26, 25 } },
        { 2, new List<int> { 40, 33, 34, 38, 32 } },
        { 3, new List<int> { 28, 29, 36, 22, 30 } },
        { 4, new List<int> { 8, 15, 21, 10, 23 } },
    };
}
