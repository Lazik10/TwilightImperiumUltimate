using MediatR;
using TwilightImperiumUltimate.Core.Models.Factions;
using TwilightImperiumUltimate.Draft.Draft.ColorDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionColorDraft;

public class ColorsDraftCommand : IRequest<IReadOnlyList<FactionColorDraftResult>>
{
    public ColorsDraftCommand(ColorDraftRequest draftRequest)
    {
        ColorDraftRequest = draftRequest;
    }

    public ColorDraftRequest ColorDraftRequest { get; }
}
