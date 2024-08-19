namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FivePlayersLargeFlatMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.FivePlayersLargeFlatMap;

    public int DimensionX => 17;

    public int DimensionY => 9;

    public int NumberOfPlayers => 5;

    public int NumberOfRedTiles => 12;

    public (int X, int Y) MecatolRexPosition => (8, 4);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (5, 7), (11, 7), (14, 4), (11, 1), (5, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 6), (0, 8), (1, 1), (1, 7), (2, 0),
        (2, 8), (14, 0), (14, 8), (15, 1), (15, 7), (16, 0), (16, 2),
        (16, 6), (16, 8),
        (0, 4), (1, 5), (2, 6), (3, 7), (4, 8),
        (12, 8), (13, 7), (14, 6), (15, 5), (16, 4),
        (15, 3), (14, 2), (13, 1), (12, 0),
        (4, 0), (3, 1), (2, 2), (1, 3),
        (4, 2), (3, 3), (2, 4), (3, 5), (4, 6),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (8, 6), (11, 5), (11, 3), (8, 2), (4, 4),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (6, 8), (7, 7), (6, 6), (8, 6), (7, 5) } },
        { 2, new List<(int X, int Y)> { (12, 6), (10, 6), (9, 7), (10, 8), (9, 5) } },
        { 3, new List<(int X, int Y)> { (13, 3), (12, 4), (13, 5), (11, 3), (11, 5) } },
        { 4, new List<(int X, int Y)> { (10, 0), (9, 1), (10, 2), (12, 2), (9, 3) } },
        { 5, new List<(int X, int Y)> { (6, 2), (7, 1), (6, 0), (8, 2), (7, 3) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (6, 8), (7, 7), (6, 6) } },
        { 2, new List<(int X, int Y)>() },
        { 3, new List<(int X, int Y)> { (13, 3), (12, 4), (13, 5) } },
        { 4, new List<(int X, int Y)>() },
        { 5, new List<(int X, int Y)> { (6, 2), (7, 1), (6, 0) } },
    };
}
