namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public static class MiltyDraftSliceSettings
{
    public static IReadOnlyCollection<int> SlicePositions { get; set; } = new List<int> { 1, 2, 3, 4, 5 };

    public static IReadOnlyCollection<int> AvailableSliceRedPositionsIfTwoAnomalies { get; set; } = new List<int> { 1, 3, 4, 5 };

    public static IReadOnlyDictionary<int, List<int>> AvailableRedPositions { get; set; } = new Dictionary<int, List<int>>()
    {
        { 1, new List<int> { 3, 5, } },
        { 3, new List<int> { 1, 4, 5, } },
        { 4, new List<int> { 3, } },
        { 5, new List<int> { 1, 3, } },
    };
}
