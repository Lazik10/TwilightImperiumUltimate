namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class EightPlayersLargeMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.EightPlayersLargeMap;

    public int DimensionX => 17;

    public int DimensionY => 9;

    public int NumberOfPlayers => 8;

    public int NumberOfRedTiles => 18;

    public (int X, int Y) MecatolRexPosition => (8, 4);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (0, 4), (3, 7), (8, 8), (13, 7), (16, 4), (13, 1), (8, 0), (3, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 6), (0, 8), (1, 1), (1, 7), (2, 0),
        (2, 8), (14, 0), (14, 8), (15, 1), (15, 7), (16, 0), (16, 2),
        (16, 6), (16, 8),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (4, 4), (6, 6), (10, 6), (12, 4), (10, 2), (6, 2),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (1, 5), (1, 3), (2, 4), (3, 5), (3, 3) } },
        { 2, new List<(int X, int Y)> { (4, 8), (5, 7), (4, 6), (2, 6), (5, 5) } },
        { 3, new List<(int X, int Y)> { (10, 8), (9, 7), (7, 7), (6, 8), (8, 6) } },
        { 4, new List<(int X, int Y)> { (14, 6), (12, 6), (11, 7), (12, 8), (11, 5) } },
        { 5, new List<(int X, int Y)> { (15, 3), (14, 4), (15, 5), (13, 3), (13, 5) } },
        { 6, new List<(int X, int Y)> { (12, 0), (11, 1), (12, 2), (14, 2), (11, 3) } },
        { 7, new List<(int X, int Y)> { (6, 0), (7, 1), (9, 1), (10, 0), (8, 2) } },
        { 8, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1), (4, 0), (5, 3) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (1, 5), (2, 4), (1, 3) } },
        { 2, new List<(int X, int Y)>() },
        { 3, new List<(int X, int Y)>() },
        { 4, new List<(int X, int Y)>() },
        { 5, new List<(int X, int Y)> { (15, 3), (14, 4), (15, 5) } },
        { 6, new List<(int X, int Y)>() },
        { 7, new List<(int X, int Y)>() },
        { 8, new List<(int X, int Y)>() },
    };
}
