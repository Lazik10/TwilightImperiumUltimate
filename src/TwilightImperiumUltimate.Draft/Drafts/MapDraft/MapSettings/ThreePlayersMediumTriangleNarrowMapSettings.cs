namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersMediumTriangleNarrowMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumTriangleNarrowMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 3;

    public int NumberOfRedTiles => 6;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>
    {
        (3, 5), (12, 4), (6, 0),
    };

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
        (4, 0), (3, 1), (2, 2), (1, 3), (4, 6), (6, 6), (8, 6), (10, 6),
        (10, 0), (11, 1), (12, 2), (13, 3),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>();

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (5, 5), (4, 4), (2, 4), (6, 4), (5, 3) } },
        { 2, new List<(int X, int Y)> { (11, 3), (10, 4), (11, 5), (9, 3), (8, 4) } },
        { 3, new List<(int X, int Y)> { (5, 1), (7, 1), (8, 0), (6, 2), (8, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (5, 5), (4, 4), (2, 4), } },
        { 2, new List<(int X, int Y)> { (11, 3), (10, 4), (11, 5), } },
        { 3, new List<(int X, int Y)> { (5, 1), (7, 1), (8, 0), } },
    };
}
