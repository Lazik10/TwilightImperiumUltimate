namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersSmallMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 3;

    public override int MaxMapPositions => 25;

    public override int MecatolRexPosition => 12;

    public override HashSet<int> HomePositions => [2, 15, 19];

    public override HashSet<int> EmptyPositions => [0, 4, 20, 21, 23, 24];
}
