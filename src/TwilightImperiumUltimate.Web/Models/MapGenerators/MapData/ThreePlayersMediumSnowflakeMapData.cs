namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersMediumSnowflakeMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumSnowflakeMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 9, 11, 18, 16 } },
        { 2, new List<int> { 40, 34, 39, 26 } },
        { 3, new List<int> { 28, 36, 22, 37 } },
    };
}
