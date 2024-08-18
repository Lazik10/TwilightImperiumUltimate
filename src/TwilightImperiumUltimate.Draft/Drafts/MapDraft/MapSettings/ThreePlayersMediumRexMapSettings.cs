namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersMediumRexMapSettings : IMapSettings, IHyperlineSettings
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersMediumRexMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 3;

    public int NumberOfRedTiles => 6;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (3, 3), (9, 5), (9, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
        (3, 1), (3, 5), (6, 0), (6, 6), (12, 2), (12, 4),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (5, 5), (11, 3), (5, 1),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X , int Y)> { (4, 4), (4, 2), (1, 3), (5, 5), (6, 4) } },
        { 2, new List<(int X , int Y)> { (10, 4), (7, 5), (10, 6), (11, 3), (9, 3) } },
        { 3, new List<(int X , int Y)> { (7, 1), (10, 2), (10, 0), (5, 1), (6, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X , int Y)>() },
        { 2, new List<(int X , int Y)>() },
        { 3, new List<(int X , int Y)>() },
    };

    public HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines => new HashSet<(int X, int Y, string SystemTileCode, string Orientation)>()
    {
        { (2, 2, "85A", "2") },
        { (2, 4, "85A", "4") },
        { (5, 3, "90A", "0") },
        { (8, 4, "90A", "2") },
        { (8, 6, "85A", "4") },
        { (11, 5, "85A", "0") },
        { (8, 2, "90A", "4") },
        { (11, 1, "85A", "0") },
        { (8, 0, "85A", "2") },
    };

    public HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors => new()
    {
        { (3, 1, 4, 4) },
        { (3, 1, 4, 2) },
        { (4, 2, 4, 4) },
        { (6, 2, 6, 4) },
        { (10, 6, 7, 5) },
        { (10, 6, 10, 4) },
        { (10, 4, 7, 5) },
        { (9, 2, 6, 4) },
        { (10, 0, 7, 1) },
        { (10, 0, 10, 2) },
        { (7, 1, 10, 2) },
        { (6, 2, 9, 3) },
    };
}
