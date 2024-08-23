using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public interface IMiltyDraftSystemTileSetter
{
    Task<List<Slice>> SetSystemTilesForSlices(SystemTilesForSlices preparedSystemTiles, List<Slice> slices);
}
