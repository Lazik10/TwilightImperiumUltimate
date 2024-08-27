namespace TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

public class EightPlayersMapPositions : IMapPositions
{
    public Dictionary<MiltyDraftInitiative, List<int>> SlicePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, new List<int> { 5, 13, 3, 14, 22 } },
        { MiltyDraftInitiative.Second, new List<int> { 25, 24, 15, 33, 23 } },
        { MiltyDraftInitiative.Third, new List<int> { 53, 42, 34, 51, 41 } },
        { MiltyDraftInitiative.Fourth, new List<int> { 69, 60, 52, 59, 50 } },
        { MiltyDraftInitiative.Fifth, new List<int> { 66, 67, 68, 57, 58 } },
        { MiltyDraftInitiative.Sixth, new List<int> { 46, 56, 65, 47, 48 } },
        { MiltyDraftInitiative.Seventh, new List<int> { 27, 38, 37, 29, 30 } },
        { MiltyDraftInitiative.Eighth, new List<int> { 11, 20, 19, 12, 21 } },
    };

    public Dictionary<MiltyDraftInitiative, int> HomePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, 4 },
        { MiltyDraftInitiative.Second, 16 },
        { MiltyDraftInitiative.Third, 44 },
        { MiltyDraftInitiative.Fourth, 61 },
        { MiltyDraftInitiative.Fifth, 74 },
        { MiltyDraftInitiative.Sixth, 55 },
        { MiltyDraftInitiative.Seventh, 36 },
        { MiltyDraftInitiative.Eighth, 10 },
    };
}
