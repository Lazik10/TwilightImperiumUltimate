namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FourPlayersMediumHyperlineMapSettings : IMapSettings, IHyperlineSettings
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumHyperlineMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 4;

    public int NumberOfRedTiles => 8;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (4, 6), (10, 6), (10, 0), (4, 0),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (3, 3), (7, 5), (11, 3), (7, 1),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (6, 6), (5, 5), (3, 5), (7, 5), (6, 4) } },
        { 2, new List<(int X, int Y)> { (11, 5), (9, 5), (8, 6), (11, 3), (8, 4) } },
        { 3, new List<(int X, int Y)> { (8, 0), (9, 1), (11, 1), (7, 1), (8, 2) } },
        { 4, new List<(int X, int Y)> { (3, 1), (5, 1), (6, 0), (3, 3), (6, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (6, 6), (5, 5), (3, 5) } },
        { 2, new List<(int X, int Y)> { (11, 5), (9, 5), (8, 6) } },
        { 3, new List<(int X, int Y)> { (8, 0), (9, 1), (11, 1) } },
        { 4, new List<(int X, int Y)> { (3, 1), (5, 1), (6, 0) } },
    };

    public HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines => new HashSet<(int X, int Y, string SystemTileCode, string Orientation)>()
    {
        { (5, 3, "86A", "3") },
        { (4, 2, "88A", "3") },
        { (2, 2, "83A", "3") },
        { (1, 3, "85A", "3") },
        { (2, 4, "84A", "3") },
        { (4, 4, "87A", "3") },

        { (9, 3, "86A", "0") },
        { (10, 4, "88A", "0") },
        { (12, 4, "83A", "0") },
        { (13, 3, "85A", "0") },
        { (12, 2, "84A", "0") },
        { (10, 2, "87A", "0") },
    };

    public HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors => new()
    {
        { (3, 3, 6, 2) },
        { (3, 3, 5, 1) },
        { (3, 3, 3, 1) },
        { (3, 3, 3, 5) },
        { (3, 3, 5, 5) },
        { (3, 3, 6, 4) },
        { (6, 2, 6, 4) },
        { (3, 1, 3, 5) },

        { (11, 3, 8, 4) },
        { (11, 3, 9, 5) },
        { (11, 3, 11, 5) },
        { (11, 3, 11, 1) },
        { (11, 3, 9, 1) },
        { (11, 3, 8, 2) },
        { (8, 4, 8, 2) },
        { (11, 1, 11, 5) },
    };
}
