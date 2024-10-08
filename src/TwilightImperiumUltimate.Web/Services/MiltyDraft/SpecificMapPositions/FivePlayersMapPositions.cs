namespace TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

public class FivePlayersMapPositions : IMapPositions
{
    public Dictionary<MiltyDraftInitiative, List<int>> SlicePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, new List<int> { 11, 10, 9, 18, 17 } },
        { MiltyDraftInitiative.Second, new List<int> { 27, 19, 12, 26, 25 } },
        { MiltyDraftInitiative.Third, new List<int> { 40, 33, 34, 38, 32 } },
        { MiltyDraftInitiative.Fourth, new List<int> { 28, 29, 36, 22, 30 } },
        { MiltyDraftInitiative.Fifth, new List<int> { 8, 15, 21, 16, 23 } },
    };

    public Dictionary<MiltyDraftInitiative, int> HomePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, 3 },
        { MiltyDraftInitiative.Second, 20 },
        { MiltyDraftInitiative.Third, 41 },
        { MiltyDraftInitiative.Fourth, 35 },
        { MiltyDraftInitiative.Fifth, 14 },
    };
}
