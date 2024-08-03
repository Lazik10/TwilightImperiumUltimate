namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

public interface IMapSettings
{
    MapTemplate MapTemplate { get; }

    int DimensionX { get; }

    int DimensionY { get; }

    int NumberOfPlayers { get; }

    (int X, int Y) MecatolRexPosition { get; }

    HashSet<(int X, int Y)> HomePositions { get; }

    HashSet<(int X, int Y)> EmptyPositions { get; }

    Dictionary<int, List<(int X, int Y)>> Slices { get; }

    Dictionary<int, List<(int X, int Y)>> AdjacentHomePositions { get; }
}
