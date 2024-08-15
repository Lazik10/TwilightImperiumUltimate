namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FourPlayersMediumMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.FourPlayersMediumMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 4;

    public int NumberOfRedTiles => 12;

    public (int, int) MecatolRexPosition => (7, 3);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>
    {
        (2, 2), (6, 6), (12, 4), (8, 3),
    };

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>
    {
        (0, 0), (0, 2), (0, 4), (0, 6), (1, 1), (1, 5), (2, 0), (2, 6),
        (12, 0), (12, 6), (13, 1), (13, 5),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>()
    {
        (4, 4), (7, 5), (10, 4), (10, 2), (7, 1), (4, 2),
    };

    public Dictionary<int, List<(int, int)>> Slices => new Dictionary<int, List<(int, int)>>()
    {
        { 1, new List<(int, int)> { (2, 4), (3, 3), (2, 2), (4, 4), (5, 3) } },
        { 2, new List<(int, int)> { (6, 6), (5, 5), (3, 5), (7, 5), (6, 4) } },
        { 3, new List<(int, int)> { (11, 5), (9, 5), (8, 6), (10, 4), (8, 4) } },
        { 4, new List<(int, int)> { (12, 2), (11, 3), (12, 4), (10, 2), (9, 3) } },
        { 5, new List<(int, int)> { (8, 0), (9, 1), (11, 1), (7, 1), (8, 2) } },
        { 6, new List<(int, int)> { (3, 1), (5, 1), (6, 0), (6, 2), (4, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => throw new NotImplementedException();
}
