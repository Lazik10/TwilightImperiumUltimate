namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class TwoPlayersMediumMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.TwoPlayersMediumMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 27, 19, 12, 26, 18, } },
        { 2, new List<int> { 28, 29, 36, 22, 37 } },
    };
}
