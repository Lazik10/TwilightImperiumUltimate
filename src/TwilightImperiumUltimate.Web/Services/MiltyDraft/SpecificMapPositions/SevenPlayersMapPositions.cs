namespace TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

public class SevenPlayersMapPositions : IMapPositions
{
    public Dictionary<MiltyDraftInitiative, List<int>> SlicePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, new List<int> { 5, 13, 3, 23, 22 } },
        { MiltyDraftInitiative.Second, new List<int> { 34, 33, 24, 42, 32 } },
        { MiltyDraftInitiative.Third, new List<int> { 60, 51, 43, 50, 41 } },
        { MiltyDraftInitiative.Fourth, new List<int> { 66, 67, 68, 57, 58 } },
        { MiltyDraftInitiative.Fifth, new List<int> { 46, 56, 65, 47, 48 } },
        { MiltyDraftInitiative.Sixth, new List<int> { 27, 38, 37, 29, 30 } },
        { MiltyDraftInitiative.Seventh, new List<int> { 11, 20, 19, 12, 21 } },
    };

    public Dictionary<MiltyDraftInitiative, int> HomePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, 4 },
        { MiltyDraftInitiative.Second, 25 },
        { MiltyDraftInitiative.Third, 52 },
        { MiltyDraftInitiative.Fourth, 74 },
        { MiltyDraftInitiative.Fifth, 55 },
        { MiltyDraftInitiative.Sixth, 36 },
        { MiltyDraftInitiative.Seventh, 10 },
    };
}
