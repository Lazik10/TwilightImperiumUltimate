using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.ColorDraft;

public interface IColorDraftService
{
    Task<FactionColorDraftResult> DraftColors(ColorDraftRequest request, CancellationToken cancellationToken);
}
