namespace TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;

public class ThreePlayersMediumMantaRayMapData : IMapData
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumMantaRayMap;

    public Dictionary<int, List<int>> SlicePositions { get; } = new()
    {
        { 1, new List<int> { 40, 33, 34, 26, 32 } },
        { 2, new List<int> { 44, 38, 46, 37, 39 } },
        { 3, new List<int> { 28, 29, 36, 22, 30 } },
    };
}
