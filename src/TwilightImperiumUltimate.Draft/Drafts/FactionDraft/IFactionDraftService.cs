using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.FactionDraft;

public interface IFactionDraftService
{
    Task<FactionDraftResult> DraftFactions(FactionDraftRequest request, CancellationToken cancellationToken);
}
