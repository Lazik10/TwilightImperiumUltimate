namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersTriangleMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 3;

    public override int MaxMapPositions => 42;

    public override int MecatolRexPosition => 24;

    public override HashSet<int> HomePositions => [3, 28, 34];

    public override HashSet<int> EmptyPositions => [0, 1, 5, 6, 7, 8, 12, 13, 14, 20, 35, 37, 39, 41];
}
