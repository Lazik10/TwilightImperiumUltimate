namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FourPlayersMediumDoubleWarpMapSettings : IMapSettings, IHyperlineSettings
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumDoubleWarpMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 4;

    public int NumberOfRedTiles => 8;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>
    {
        (4, 6), (10, 6), (10, 0), (4, 0),
    };

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
        (1, 3), (2, 2), (2, 4), (12, 2), (12, 4), (13, 3),
        (5, 3), (9, 3),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>()
    {
        (3, 3), (7, 5), (11, 3), (7, 1),
    };

    public Dictionary<int, List<(int, int)>> Slices => new Dictionary<int, List<(int, int)>>()
    {
        { 1, new List<(int, int)> { (6, 6), (5, 5), (3, 5), (6, 4) } },
        { 2, new List<(int, int)> { (11, 5), (9, 5), (8, 6), (8, 4) } },
        { 3, new List<(int, int)> { (8, 0), (9, 1), (11, 1), (8, 2) } },
        { 4, new List<(int, int)> { (3, 1), (5, 1), (6, 0), (6, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int, int)>>()
    {
        { 1, new List<(int, int)> { (6, 6), (5, 5), (3, 5), } },
        { 2, new List<(int, int)> { (11, 5), (9, 5), (8, 6), } },
        { 3, new List<(int, int)> { (8, 0), (9, 1), (11, 1), } },
        { 4, new List<(int, int)> { (3, 1), (5, 1), (6, 0), } },
    };

    public HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines => new HashSet<(int X, int Y, string SystemTileCode, string Orientation)>()
    {
        { (4, 2, "91B", "1") },
        { (4, 4, "89B", "5") },
        { (10, 4, "87B", "4") },
        { (10, 2, "90B", "2") },
    };

    public HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors => new()
    {
        { (3, 3, 5, 1) },
        { (3, 3, 6, 2) },
        { (3, 3, 6, 4) },
        { (3, 3, 5, 5) },
        { (11, 3, 9, 1) },
        { (11, 3, 8, 2) },
        { (11, 3, 8, 4) },
        { (11, 3, 9, 5) },
    };
}
