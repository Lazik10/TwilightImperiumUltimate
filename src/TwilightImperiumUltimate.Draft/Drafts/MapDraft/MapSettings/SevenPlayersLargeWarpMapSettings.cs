namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class SevenPlayersLargeWarpMapSettings : IMapSettings, IHyperlineSettings
{
    public MapTemplate MapTemplate => MapTemplate.SevenPlayersLargeWarpMap;

    public int DimensionX => 17;

    public int DimensionY => 9;

    public int NumberOfPlayers => 7;

    public (int X, int Y) MecatolRexPosition => (8, 4);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (0, 4), (5, 7), (11, 7), (16, 4), (13, 1), (8, 0), (3, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        // TODO: Check all empty positions count
        (0, 0), (0, 2), (0, 6), (0, 8), (1, 1), (1, 7), (2, 0),
        (2, 8), (14, 0), (14, 8), (15, 1), (15, 7), (16, 0), (16, 2),
        (16, 6), (16, 8), (2, 6), (3, 7), (4, 8), (6, 8), (10, 8), (12, 8),
        (14, 8), (15, 7), (14, 6), (12, 0), (10, 0), (4, 0),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (3, 3), (5, 5), (8, 6), (11, 5), (13, 3), (10, 2), (6, 2),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (1, 5), (2, 4), (1, 3), (5, 5), (4, 4) } },
        { 2, new List<(int X, int Y)> { (7, 7), (6, 6), (4, 6), (8, 4), (7, 5) } },
        { 3, new List<(int X, int Y)> { (12, 6), (10, 6), (9, 7), (11, 5), (9, 5) } },
        { 4, new List<(int X, int Y)> { (15, 3), (14, 4), (15, 5), (13, 3), (12, 4) } },
        { 5, new List<(int X, int Y)> { (11, 1), (12, 2), (14, 2), (10, 2), (11, 3) } },
        { 6, new List<(int X, int Y)> { (6, 0), (8, 2), (9, 1), (6, 2), (7, 3) } },
        { 7, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1), (3, 3), (5, 3) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (1, 5), (2, 4), (1, 3) } },
        { 2, new List<(int X, int Y)> { (7, 7), (6, 6), (4, 6) } },
        { 3, new List<(int X, int Y)> { (12, 6), (10, 6), (9, 7) } },
        { 4, new List<(int X, int Y)> { (15, 3), (14, 4), (15, 5) } },
        { 5, new List<(int X, int Y)> { (11, 1), (12, 2), (14, 2) } },
        { 6, new List<(int X, int Y)> { (6, 0), (8, 2), (9, 1) } },
        { 7, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1) } },
    };

    public HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines => new HashSet<(int X, int Y, string SystemTileCode, string Orientation)>()
    {
        { (3, 5, "88B", "0") },
        { (6, 4, "85B", "0") },
        { (7, 1, "83B", "2") },
        { (9, 3, "90B", "0") },
        { (10, 4, "84B", "0") },
        { (13, 5, "86B", "0") },
    };

    public HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors => new()
    {
        { (1, 5, 5, 5) },
        { (2, 4, 5, 5) },
        { (1, 5, 4, 6) },
        { (4, 4, 8, 4) },
        { (4, 4, 7, 5) },
        { (5, 3, 8, 4) },
        { (6, 0, 6, 2) },
        { (6, 0, 8, 2) },
        { (8, 0, 8, 2) },
        { (7, 3, 11, 3) },
        { (7, 3, 10, 2) },
        { (12, 4, 8, 4) },
        { (12, 4, 9, 5) },
        { (11, 3, 8, 4) },
        { (11, 5, 15, 5) },
        { (11, 5, 14, 4) },
        { (12, 6, 15, 5) },
    };
}
