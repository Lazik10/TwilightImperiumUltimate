namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class FourPlayersMediumGapsMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumGapsMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 19, 18, 11, 25 } },
        { 2, new List<int> { 46, 39, 33, 32 } },
        { 3, new List<int> { 29, 37, 44, 30, } },
        { 4, new List<int> { 9, 16, 15, 23, } },
    };
}
