namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class SixPlayersMediumSpiralMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.SixPlayersMediumSpiralMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 6;

    public int NumberOfRedTiles => 12;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (2, 4), (6, 6), (11, 5), (12, 2), (8, 0), (3, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>();

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X , int Y)> { (3, 5), (4, 4), (3, 3), (1, 3), (5, 3) } },
        { 2, new List<(int X , int Y)> { (8, 6), (7, 5), (5, 5), (4, 6), (6, 4) } },
        { 3, new List<(int X , int Y)> { (12, 4), (10, 4), (9, 5), (10, 6), (8, 4) } },
        { 4, new List<(int X , int Y)> { (11, 1), (10, 2), (11, 3), (13, 3), (9, 3) } },
        { 5, new List<(int X , int Y)> { (6, 0), (7, 1), (9, 1), (10, 0), (8, 2) } },
        { 6, new List<(int X , int Y)> { (2, 2), (4, 2), (5, 1), (4, 0), (6, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X , int Y)> { (3, 5), (4, 4), (3, 3), (1, 3) } },
        { 2, new List<(int X , int Y)> { (8, 6), (7, 5), (5, 5), (4, 6) } },
        { 3, new List<(int X , int Y)> { (12, 4), (10, 4), (9, 5), (10, 6) } },
        { 4, new List<(int X , int Y)> { (11, 1), (10, 2), (11, 3), (13, 3) } },
        { 5, new List<(int X , int Y)> { (6, 0), (7, 1), (9, 1), (10, 0) } },
        { 6, new List<(int X , int Y)> { (2, 2), (4, 2), (5, 1), (4, 0) } },
    };
}
