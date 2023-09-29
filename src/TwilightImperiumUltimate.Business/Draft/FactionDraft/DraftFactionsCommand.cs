using MediatR;
using TwilightImperiumUltimate.API.Models.Factions;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionDraft;

public class DraftFactionsCommand : IRequest<List<DraftResult>>
{
    public DraftFactionsCommand(FactionDraftRequest draftRequest)
    {
        FactionDraftRequest = draftRequest;
    }

    public FactionDraftRequest FactionDraftRequest { get; }
}
