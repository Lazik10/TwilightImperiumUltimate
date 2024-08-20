namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FourPlayersMediumVerticalMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumVerticalMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 4;

    public int NumberOfRedTiles => 8;

    public (int X, int Y) MecatolRexPosition => (7, 3);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (3, 5), (11, 5), (11, 1), (3, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
        (1, 3), (13, 3), (4, 6), (6, 6), (8, 6), (10, 6), (10, 0), (8, 0), (6, 0), (4, 0),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>()
    {
        (3, 3), (7, 5), (11, 3), (7, 1),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (5, 5), (4, 4), (2, 4), (6, 4), } },
        { 2, new List<(int X, int Y)> { (12, 4), (10, 4), (9, 5), (8, 4), } },
        { 3, new List<(int X, int Y)> { (9, 1), (10, 2), (12, 2), (8, 2), } },
        { 4, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1), (6, 2), } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (5, 5), (4, 4), (2, 4),  } },
        { 2, new List<(int X, int Y)> { (12, 4), (10, 4), (9, 5), } },
        { 3, new List<(int X, int Y)> { (9, 1), (10, 2), (12, 2), } },
        { 4, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1), } },
    };
}
