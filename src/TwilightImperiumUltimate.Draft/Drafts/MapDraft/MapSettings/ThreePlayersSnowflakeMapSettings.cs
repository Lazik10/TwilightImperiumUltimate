namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersSnowflakeMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 3;

    public override int MaxMapPositions => 42;

    public override int MecatolRexPosition => 24;

    public override HashSet<int> HomePositions => [10, 29, 33];

    public override HashSet<int> EmptyPositions => [0, 1, 3, 5, 6, 7, 13, 17, 23, 25, 28, 34, 35, 41];
}
