namespace TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

public interface IMapPositions
{
    public Dictionary<MiltyDraftInitiative, List<int>> SlicePositions { get; }

    public Dictionary<MiltyDraftInitiative, int> HomePositions { get; }
}
