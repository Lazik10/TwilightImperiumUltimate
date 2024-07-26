namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal abstract class MapSettingsBase : IMapSettings
{
    public virtual int MecatolRexPosition { get; }

    public virtual HashSet<int> HomePositions { get; } = [];

    public virtual HashSet<int> EmptyPositions { get; } = [];

    public virtual int MaxMapPositions { get; }

    public virtual int NumberOfPlayers { get; }

    public bool IsHomeSpot(int position) => HomePositions.Contains(position);

    public bool IsEmptySpot(int position) => EmptyPositions.Contains(position);
}
