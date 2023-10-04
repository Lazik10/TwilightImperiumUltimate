using MediatR;
using TwilightImperiumUltimate.Core.Models.Factions;
using TwilightImperiumUltimate.Draft.Draft.ColorDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionColorDraft;

public class DraftColorsCommand : IRequest<IReadOnlyList<FactionColorDraftResult>>
{
    public DraftColorsCommand(ColorDraftRequest draftRequest)
    {
        ColorDraftRequest = draftRequest;
    }

    public ColorDraftRequest ColorDraftRequest { get; }
}
