using MediatR;
using TwilightImperiumUltimate.API.Models.Factions;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionDraft;

public class FactionsDraftCommand : IRequest<List<FactionDraftResult>>
{
    public FactionsDraftCommand(FactionDraftRequest draftRequest)
    {
        FactionDraftRequest = draftRequest;
    }

    public FactionDraftRequest FactionDraftRequest { get; }
}
