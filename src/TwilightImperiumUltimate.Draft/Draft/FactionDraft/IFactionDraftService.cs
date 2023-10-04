using TwilightImperiumUltimate.API.Models.Factions;

namespace TwilightImperiumUltimate.Draft.Draft.FactionDraft;

public interface IFactionDraftService
{
    Task<List<FactionDraftResult>> DraftFactionsAsync(FactionDraftRequest draftRequest);
}
