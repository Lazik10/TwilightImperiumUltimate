using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public interface ISliceSystemTilePreparer
{
    Task<SystemTilesForSlices> PrepareSystemTilesForSlices(
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForSlices systemTilesForSliceRedistribution,
        SliceDraftRequest request);
}
