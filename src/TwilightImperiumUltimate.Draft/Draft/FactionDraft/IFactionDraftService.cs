using TwilightImperiumUltimate.API.Models.Factions;

namespace TwilightImperiumUltimate.Draft.Draft.FactionDraft;

public interface IFactionDraftService
{
    Task<List<DraftResult>> DraftFactionsAsync(FactionDraftRequest draftRequest);
}
