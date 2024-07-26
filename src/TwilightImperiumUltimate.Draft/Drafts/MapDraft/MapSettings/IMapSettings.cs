namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

public interface IMapSettings
{
    int NumberOfPlayers { get; }

    int MaxMapPositions { get; }

    int MecatolRexPosition { get; }

    HashSet<int> HomePositions { get; }

    HashSet<int> EmptyPositions { get; }

    bool IsHomeSpot(int position);

    bool IsEmptySpot(int position);
}
