using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public interface IMiltyDraftSliceBalancer
{
    Task<List<Slice>> BalanceSlices(
        SystemTilesForSlices systemTilesForSlices,
        List<Slice> slices);
}
