namespace TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

public interface IMapPositions
{
    Dictionary<MiltyDraftInitiative, List<int>> SlicePositions { get; }

    Dictionary<MiltyDraftInitiative, int> HomePositions { get; }
}
