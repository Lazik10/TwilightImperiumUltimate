namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

internal class ThreePlayersMantaRayMapSettings : MapSettingsBase
{
    public override int NumberOfPlayers => 3;

    public override int MaxMapPositions => 35;

    public override int MecatolRexPosition => 10;

    public override HashSet<int> HomePositions => [14, 20, 31];

    public override HashSet<int> EmptyPositions => [0, 1, 5, 6, 21, 27, 28, 29, 30, 32, 33, 34];
}
