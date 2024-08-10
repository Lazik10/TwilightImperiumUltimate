namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class CustomMapSettings : IMapSettings
{
    public MapTemplate MapTemplate => MapTemplate.CustomMap;

    public int DimensionX => 14;

    public int DimensionY => 7;

    public int NumberOfPlayers => -1;

    public (int, int) MecatolRexPosition => (-1, -1);

    public HashSet<(int, int)> HomePositions => new HashSet<(int, int)>();

    public HashSet<(int, int)> EmptyPositions => new HashSet<(int, int)>();

    public HashSet<(int X, int Y)> EquidistantPositions => new HashSet<(int X, int Y)>();

    public Dictionary<int, List<(int, int)>> Slices => new Dictionary<int, List<(int, int)>>();

    public Dictionary<int, List<(int, int)>> AdjacentHomePositions => new Dictionary<int, List<(int, int)>>();
}
