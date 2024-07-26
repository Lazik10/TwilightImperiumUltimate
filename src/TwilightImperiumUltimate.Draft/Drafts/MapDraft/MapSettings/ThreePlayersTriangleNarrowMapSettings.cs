namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersTriangleNarrowMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 3;

    public override int MaxMapPositions => 36;

    public override int MecatolRexPosition => 21;

    public override HashSet<int> HomePositions => [11, 12, 34];

    public override HashSet<int> EmptyPositions => [0, 1, 2, 3, 5, 6, 7, 24, 30, 31, 32];
}
