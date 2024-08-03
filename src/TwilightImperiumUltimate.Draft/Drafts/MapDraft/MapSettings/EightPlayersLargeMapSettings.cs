
namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class EightPlayersLargeMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.SixPlayersMediumMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => 8;

    public (int X, int Y) MecatolRexPosition => (8, 4);

    public HashSet<(int X, int Y)> HomePositions => new HashSet<(int X, int Y)>
    {
        (0, 4), (4, 0), (4, 8), (12, 0), (12, 8), (16, 4),
    };

    public HashSet<(int X, int Y)> EmptyPositions => new HashSet<(int X, int Y)>
    {
        (0, 0), (0, 2), (0, 6), (0, 8), (1, 1), (1, 7), (2, 0), (2, 8),
        (14, 0), (14, 8), (15, 1), (15, 7), (16, 0), (16, 2), (16, 6), (16, 8),
    };

    public Dictionary<int, List<(int X, int Y)>> Slices => new Dictionary<int, List<(int X, int Y)>>();

    public Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions => new Dictionary<int, List<(int X, int Y)>>();
}
