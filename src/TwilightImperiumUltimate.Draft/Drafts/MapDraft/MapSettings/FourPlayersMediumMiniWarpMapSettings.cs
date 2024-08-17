namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FourPlayersMediumMiniWarpMapSettings : IMapSettings, IHyperlineSettings
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumMiniWarpMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 4;

    public int NumberOfRedTiles => 11;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>
    {
        (2, 2), (6, 6), (12, 4), (8, 0),
    };

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>()
    {
        (5, 1), (9, 5),
    };

    public Dictionary<int, List<(int, int)>> Slices => new Dictionary<int, List<(int, int)>>()
    {
        { 1, new List<(int, int)> { (8, 6), (7, 5), (5, 5), (4, 6), (6, 4) } },
        { 2, new List<(int, int)> { (13, 3), (11, 3), (10, 4), (11, 5), (9, 3) } },
        { 3, new List<(int, int)> { (6, 0), (7, 1), (9, 1), (10, 0), (8, 2) } },
        { 4, new List<(int, int)> { (1, 3), (3, 3), (4, 2), (3, 1), (5, 3) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int, int)>>()
    {
        { 1, new List<(int, int)>() },
        { 2, new List<(int, int)>() },
        { 3, new List<(int, int)>() },
        { 4, new List<(int, int)>() },
    };

    public HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines => new HashSet<(int X, int Y, string SystemTileCode, string Orientation)>()
    {
        { (4, 4, "91A", "2") },
        { (10, 2, "84B", "2") },
        { (4, 0, "86A", "2") },
        { (10, 6, "85A", "5") },
    };

    public HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors => new()
    {
        { (6, 0, 3, 1) },
        { (8, 6, 11, 5) },
        { (3, 3, 6, 4) },
        { (3, 3, 5, 5) },
        { (5, 5, 2, 4) },
        { (9, 1, 11, 3) },
        { (9, 1, 12, 2) },
        { (11, 3, 8, 2) },
    };
}
