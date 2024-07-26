namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class SixPlayersLargeMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 6;

    public override int MaxMapPositions => 81;

    public override int MecatolRexPosition => 40;

    public override HashSet<int> HomePositions => [4, 26, 62, 76, 54, 18];

    public override HashSet<int> EmptyPositions => [0, 1, 2, 6, 7, 8, 9, 17, 78, 77, 75, 74, 73, 72, 71, 70, 64, 63, 79, 80];
}
