namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FourPlayersMediumHorizontalMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumHorizontalMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 4;

    public int NumberOfRedTiles => 9;

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
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>()
    {
        (7, 5), (11, 3), (7, 1), (3, 3),
    };

    public Dictionary<int, List<(int, int)>> Slices => new Dictionary<int, List<(int, int)>>()
    {
        { 1, new List<(int, int)> { (6, 6), (5, 5), (3, 5), (4, 4), (6, 4) } },
        { 2, new List<(int, int)> { (11, 5), (9, 5), (8, 6), (10, 4), (8, 4) } },
        { 3, new List<(int, int)> { (8, 0), (9, 1), (11, 1), (10, 2), (8, 2) } },
        { 4, new List<(int, int)> { (3, 1), (5, 1), (6, 0), (4, 2), (6, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int, int)>>()
    {
        { 1, new List<(int, int)> { (6, 6), (5, 5), (3, 5), } },
        { 2, new List<(int, int)> { (11, 5), (9, 5), (8, 6), } },
        { 3, new List<(int, int)> { (8, 0), (9, 1), (11, 1), } },
        { 4, new List<(int, int)> { (3, 1), (5, 1), (6, 0), } },
    };
}
