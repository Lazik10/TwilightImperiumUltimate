namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class EightPlayersLargeWarpMapSettings : IMapSettings, IHyperlineSettings
{
    public MapTemplate MapTemplate => MapTemplate.EightPlayersLargeMap;

    public int DimensionX => 17;

    public int DimensionY => 9;

    public int NumberOfPlayers => 8;

    public (int X, int Y) MecatolRexPosition => (8, 4);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (0, 4), (3, 7), (8, 8), (13, 7), (16, 4), (13, 1), (8, 0), (3, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 6), (0, 8), (1, 1), (1, 7), (2, 0),
        (2, 8), (14, 0), (14, 8), (15, 1), (15, 7), (16, 0), (16, 2),
        (16, 6), (16, 8), (4, 8), (6, 8), (12, 8), (12, 0), (10, 0), (4, 0),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (3, 5), (6, 6), (10, 6), (12, 4), (13, 5), (13, 3), (10, 2), (6, 2), (3, 3),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (1, 5), (2, 4), (1, 3), (3, 5), (4, 4) } },
        { 2, new List<(int X, int Y)> { (5, 7), (4, 6), (2, 6), (6, 6), (5, 5) } },
        { 3, new List<(int X, int Y)> { (10, 8), (8, 6), (7, 7), (10, 6), (9, 5) } },
        { 4, new List<(int X, int Y)> { (14, 6), (12, 6), (11, 7), (13, 5), (11, 5) } },
        { 5, new List<(int X, int Y)> { (15, 3), (14, 4), (15, 5), (13, 3), (12, 4) } },
        { 6, new List<(int X, int Y)> { (11, 1), (12, 2), (14, 2), (10, 2), (11, 3) } },
        { 7, new List<(int X, int Y)> { (6, 0), (8, 2), (9, 1), (6, 2), (7, 3) } },
        { 8, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1), (3, 3), (5, 3) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>
    {
        { 1, new List<(int X, int Y)> { (1, 5), (2, 4), (1, 3) } },
        { 2, new List<(int X, int Y)> { (5, 7), (4, 6), (2, 6) } },
        { 3, new List<(int X, int Y)> { (10, 8), (8, 6), (7, 7) } },
        { 4, new List<(int X, int Y)> { (14, 6), (12, 6), (11, 7) } },
        { 5, new List<(int X, int Y)> { (15, 3), (14, 4), (15, 5) } },
        { 6, new List<(int X, int Y)> { (11, 1), (12, 2), (14, 2) } },
        { 7, new List<(int X, int Y)> { (6, 0), (8, 2), (9, 1) } },
        { 8, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1) } },
    };

    public HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines => new HashSet<(int X, int Y, string SystemTileCode, string Orientation)>()
    {
        { (6, 4, "87A", "1") },
        { (7, 5, "90B", "3") },
        { (10, 4, "88A", "2") },
        { (9, 3, "89B", "0") },
        { (9, 7, "85B", "2") },
        { (7, 1, "83B", "2") },
    };

    public HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors => new()
    {
        { (4, 4, 8, 4) },
        { (12, 4, 8, 4) },
        { (5, 3, 8, 4) },
        { (5, 5, 8, 4) },
        { (11, 3, 8, 4) },
        { (11, 5, 8, 4) },
        { (9, 5, 5, 5) },
        { (9, 5, 6, 6) },
        { (7, 3, 11, 3) },
        { (7, 3, 10, 2) },
        { (8, 6, 8, 8) },
        { (8, 6, 10, 8) },
        { (10, 6, 10, 8) },
        { (8, 2, 8, 0) },
        { (8, 2, 6, 0) },
        { (6, 2, 6, 0) },
    };
}
