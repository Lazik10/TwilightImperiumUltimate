namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FivePlayersMediumDiamondMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.FivePlayersMediumMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 5;

    public int NumberOfRedTiles => 10;

    public (int X, int Y) MecatolRexPosition => (7, 3);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (3, 5), (8, 6), (13, 3), (8, 0), (3, 1),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
        (4, 6), (10, 6), (11, 5), (11, 1), (10, 0), (4, 0),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (3, 3), (6, 4), (10, 4), (10, 2), (6, 2),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (5, 5), (4, 4), (2, 4), (6, 4), } },
        { 2, new List<(int X, int Y)> { (9, 5), (7, 5), (6, 6), (8, 4), } },
        { 3, new List<(int X, int Y)> { (12, 2), (11, 3), (12, 4), (9, 3), } },
        { 4, new List<(int X, int Y)> { (6, 0), (7, 1), (9, 1), (8, 2), } },
        { 5, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1), (6, 2), } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (5, 5), (4, 4), (2, 4), } },
        { 2, new List<(int X, int Y)> { (9, 5), (7, 5), (6, 6), } },
        { 3, new List<(int X, int Y)> { (12, 2), (11, 3), (12, 4), } },
        { 4, new List<(int X, int Y)> { (6, 0), (7, 1), (9, 1), } },
        { 5, new List<(int X, int Y)> { (2, 2), (4, 2), (5, 1), } },
    };
}
