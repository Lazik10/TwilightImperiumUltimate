namespace TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

public class ThreePlayersMapPositions : IMapPositions
{
    public Dictionary<MiltyDraftInitiative, List<int>> SlicePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, new List<int> { 11, 10, 9, 19, 17 } },
        { MiltyDraftInitiative.Second, new List<int> { 40, 33, 34, 38, 32 } },
        { MiltyDraftInitiative.Third, new List<int> { 28, 29, 36, 15, 30 } },
    };

    public Dictionary<MiltyDraftInitiative, int> HomePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, 3 },
        { MiltyDraftInitiative.Second, 41 },
        { MiltyDraftInitiative.Third, 35 },
    };
}
