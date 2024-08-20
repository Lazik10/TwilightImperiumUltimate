namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersSmallMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.ThreePlayersSmallMap;

    public int DimensionX => 9;

    public int DimensionY => 5;

    public int NumberOfPlayers => 3;

    public int NumberOfRedTiles => 6;

    public (int, int) MecatolRexPosition => (4, 2);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>
    {
        (0, 2), (6, 4), (6, 0),
    };

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>
    {
        (0, 0), (0, 4), (8, 0), (8, 4),
    };

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>
    {
        (3, 1), (3, 3), (6, 2),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (1, 3), (2, 2), (1, 1), (2, 0), (3, 1) } },
        { 2, new List<(int X, int Y)> { (7, 3), (5, 3), (4, 4), (2, 4), (3, 3) } },
        { 3, new List<(int X, int Y)> { (4, 0), (5, 1), (7, 1), (8, 2), (6, 2) } },
    };

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>()
    {
        { 1, new List<(int X, int Y)> { (1, 3), (2, 2), (1, 1), } },
        { 2, new List<(int X, int Y)> { (7, 3), (5, 3), (4, 4), } },
        { 3, new List<(int X, int Y)> { (4, 0), (5, 1), (7, 1), } },
    };
}
