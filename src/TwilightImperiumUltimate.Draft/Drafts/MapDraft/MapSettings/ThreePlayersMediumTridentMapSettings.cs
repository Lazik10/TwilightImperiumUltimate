namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersMediumTridentMapSettings : IMapSettings, IHyperlineSettings
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumTridentMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 3;

    public int NumberOfRedTiles => 6;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (1, 3), (10, 6), (10, 0),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (5, 5), (11, 3), (5, 1),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (2, 4), (3, 3), (2, 2), (5, 5), (5, 3) } },
        { 2, new List<(int X, int Y)> { (11, 5), (9, 5), (8, 6), (11, 3), (8, 4) } },
        { 3, new List<(int X, int Y)> { (8, 0), (9, 1), (11, 1), (5, 1), (8, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (2, 4), (3, 3), (2, 2), } },
        { 2, new List<(int X, int Y)> { (11, 5), (9, 5), (8, 6), } },
        { 3, new List<(int X, int Y)> { (8, 0), (9, 1), (11, 1), } },
    };

    public HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines => new HashSet<(int X, int Y, string SystemTileCode, string Orientation)>()
    {
        { (9, 3, "86A", "0") },
        { (10, 4, "88A", "0") },
        { (12, 4, "83A", "0") },
        { (13, 3, "85A", "0") },
        { (12, 2, "84A", "0") },
        { (10, 2, "87A", "0") },
        { (6, 4, "86A", "4") },
        { (4, 4, "88A", "4") },
        { (3, 5, "83A", "1") },
        { (4, 6, "85A", "4") },
        { (6, 6, "84A", "1") },
        { (7, 5, "87A", "4") },
        { (6, 2, "86A", "2") },
        { (7, 1, "88A", "2") },
        { (6, 0, "83A", "2") },
        { (4, 0, "85A", "2") },
        { (3, 1, "84A", "2") },
        { (4, 2, "87A", "2") },
    };

    public HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors => new()
    {
        { (11, 3, 8, 4) },
        { (11, 3, 9, 5) },
        { (11, 3, 11, 5) },
        { (11, 3, 11, 1) },
        { (11, 3, 9, 1) },
        { (11, 3, 8, 2) },
        { (8, 4, 8, 2) },
        { (11, 1, 11, 5) },
        { (5, 5, 5, 3) },
        { (5, 5, 3, 3) },
        { (5, 5, 2, 4) },
        { (5, 5, 8, 4) },
        { (5, 5, 9, 5) },
        { (5, 5, 8, 6) },
        { (2, 4, 8, 6) },
        { (5, 3, 8, 4) },
        { (5, 1, 2, 2) },
        { (5, 1, 3, 3) },
        { (5, 1, 5, 3) },
        { (5, 1, 8, 2) },
        { (5, 1, 9, 1) },
        { (5, 1, 8, 0) },
        { (8, 0, 2, 2) },
        { (5, 3, 8, 2) },
    };
}
