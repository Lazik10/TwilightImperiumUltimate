namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class EightPlayersLargeMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 8;

    public override int MaxMapPositions => 81;

    public override int MecatolRexPosition => 40;

    public override HashSet<int> HomePositions => [4, 10, 16, 36, 44, 55, 61, 76];

    public override HashSet<int> EmptyPositions => [0, 1, 2, 6, 7, 8, 9, 17, 78, 77, 75, 74, 73, 72, 71, 70, 64, 63, 79, 80];
}
