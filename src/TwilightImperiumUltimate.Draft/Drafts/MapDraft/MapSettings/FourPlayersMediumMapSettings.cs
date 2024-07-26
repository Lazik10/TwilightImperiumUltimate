namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class FourPlayersMediumMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 4;

    public override int MaxMapPositions => 49;

    public override int MecatolRexPosition => 24;

    public override HashSet<int> HomePositions => [8, 12, 36, 40];

    public override HashSet<int> EmptyPositions => [0, 1, 5, 6, 35, 41, 42, 43, 44, 46, 47, 48];
}
