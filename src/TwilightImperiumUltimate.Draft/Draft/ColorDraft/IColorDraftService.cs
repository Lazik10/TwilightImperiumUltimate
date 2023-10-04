using TwilightImperiumUltimate.Core.Models.Factions;

namespace TwilightImperiumUltimate.Draft.Draft.ColorDraft;

public interface IColorDraftService
{
    Task<IReadOnlyList<FactionColorDraftResult>> DraftColorsAsync(ColorDraftRequest draftRequest);
}
