namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

public interface IHyperlineSettings
{
    HashSet<(int X, int Y, string SystemTileCode, string Orientation)> Hyperlines { get; }

    HashSet<(int X1, int Y1, int X2, int Y2)> CustomNeighbors { get; }
}
