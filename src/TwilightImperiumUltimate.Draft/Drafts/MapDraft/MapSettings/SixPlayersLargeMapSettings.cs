namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class SixPlayersLargeMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.SixPlayersLargeMap;

    public int DimensionX => 17;

    public int DimensionY => 9;

    public int NumberOfPlayers => 6;

    public int NumberOfRedTiles => 18;

    public (int, int) MecatolRexPosition => (8, 4);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>
    {
        (0, 4), (4, 8), (12, 8), (16, 4), (12, 0), (4, 0),
    };

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>
    {
        (0, 0), (0, 2), (0, 6), (0, 8), (1, 1), (1, 7), (2, 0),
        (2, 8), (14, 0), (14, 8), (15, 1), (15, 7), (16, 0), (16, 2),
        (16, 6), (16, 8),
    };

    public HashSet<(int, int)> EquidistantPositions => new HashSet<(int, int)>
    {
        (5, 5), (8, 6), (11, 5), (11, 3), (8, 2), (5, 3),
    };

    public Dictionary<int, List<(int, int)>> Slices => new Dictionary<int, List<(int, int)>>
    {
        { 1, new List<(int, int)>() { (1, 5), (2, 4), (1, 3), (3, 5), (3, 3) } },
        { 2, new List<(int, int)>() { (6, 8), (5, 7), (3, 7), (7, 7), (4, 6) } },
        { 3, new List<(int, int)>() { (13, 7), (11, 7), (10, 8), (12, 6), (9, 7) } },
        { 4, new List<(int, int)>() { (15, 3), (14, 4), (15, 5), (13, 3), (13, 5) } },
        { 5, new List<(int, int)>() { (10, 0), (11, 1), (13, 1), (9, 1), (12, 2) } },
        { 6, new List<(int, int)>() { (3, 1), (5, 1), (6, 0), (4, 2), (7, 1) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int, int)>() { (1, 5), (2, 4), (1, 3) } },
        { 2, new List<(int, int)>() { (6, 8), (5, 7), (3, 7) } },
        { 3, new List<(int, int)>() { (13, 7), (11, 7), (10, 8) } },
        { 4, new List<(int, int)>() { (15, 3), (14, 4), (15, 5) } },
        { 5, new List<(int, int)>() { (10, 0), (11, 1), (13, 1) } },
        { 6, new List<(int, int)>() { (3, 1), (5, 1), (6, 0) } },
    };
}
