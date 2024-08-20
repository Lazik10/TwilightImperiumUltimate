namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class TwoPlayersMediumMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.TwoPlayersMediumMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 2;

    public int NumberOfRedTiles => 8;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (4, 6), (10, 0),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
        (1, 3), (2, 2), (2, 4), (3, 1), (5, 1), (4, 0), (6, 0),
        (13, 3), (12, 2), (12, 4), (11, 5), (9, 5), (10, 6), (8, 6),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (4, 2), (10, 4),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (6, 6), (5, 5), (3, 5), (7, 5), (4, 4) } },
        { 2, new List<(int X, int Y)> { (8, 0), (9, 1), (11, 1), (7, 1), (10, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (6, 6), (5, 5), (3, 5), } },
        { 2, new List<(int X, int Y)> { (8, 0), (9, 1), (11, 1), } },
    };
}
