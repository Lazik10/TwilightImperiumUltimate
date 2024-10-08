namespace TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

public class FourPlayersMapPositions : IMapPositions
{
    public Dictionary<MiltyDraftInitiative, List<int>> SlicePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, new List<int> { 27, 19, 12, 26, 25 } },
        { MiltyDraftInitiative.Second, new List<int> { 40, 33, 34, 38, 32 } },
        { MiltyDraftInitiative.Third, new List<int> { 28, 29, 36, 22, 30 } },
        { MiltyDraftInitiative.Fourth, new List<int> { 8, 15, 21, 10, 23 } },
    };

    public Dictionary<MiltyDraftInitiative, int> HomePositions { get; } = new()
    {
        { MiltyDraftInitiative.Speaker, 20 },
        { MiltyDraftInitiative.Second, 41 },
        { MiltyDraftInitiative.Third, 35 },
        { MiltyDraftInitiative.Fourth, 14 },
    };
}
