using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public interface IDraftSlicesService
{
    Task<List<Slice>> DraftSlices(
        SliceDraftRequest request,
        CancellationToken cancellationToken);
}
