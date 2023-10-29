using MediatR;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionDraft;

internal class FactionsDraftCommandHandler : IRequestHandler<FactionsDraftCommand, List<FactionDraftResult>>
{
    private readonly IFactionDraftService _factionDraftService;

    public FactionsDraftCommandHandler(IFactionDraftService factionDraftService)
    {
        _factionDraftService = factionDraftService;
    }

    public Task<List<FactionDraftResult>> Handle(FactionsDraftCommand command, CancellationToken cancellationToken)
    {
        return _factionDraftService.DraftFactionsAsync(command.FactionDraftRequest);
    }
}
