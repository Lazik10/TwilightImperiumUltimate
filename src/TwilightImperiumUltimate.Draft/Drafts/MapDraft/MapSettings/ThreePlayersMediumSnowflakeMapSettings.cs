namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersMediumSnowflakeMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumSnowflakeMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 3;

    public int NumberOfRedTiles => 8;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>
    {
        (3, 3), (9, 5), (9, 1),
    };

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
        (1, 3), (4, 0), (4, 6), (5, 3), (8, 2), (8, 4), (10, 0), (10, 6), (13, 3),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (5, 1), (5, 5), (11, 3),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X , int Y)> { (10, 4), (7, 5), (11, 5), (8, 6), } },
        { 2, new List<(int X , int Y)> { (7, 1), (10, 2), (8, 0), (11, 1), } },
        { 3, new List<(int X , int Y)> { (4, 4), (4, 2), (2, 4), (2, 2), } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X , int Y)>() },
        { 2, new List<(int X , int Y)>() },
        { 3, new List<(int X , int Y)>() },
    };
}
