using MediatR;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionDraft;

internal class DraftFactionsCommandHandler : IRequestHandler<DraftFactionsCommand, List<FactionDraftResult>>
{
    private readonly IFactionDraftService _factionDraftService;

    public DraftFactionsCommandHandler(IFactionDraftService factionDraftService)
    {
        _factionDraftService = factionDraftService;
    }

    public Task<List<FactionDraftResult>> Handle(DraftFactionsCommand command, CancellationToken cancellationToken)
    {
        return _factionDraftService.DraftFactionsAsync(command.FactionDraftRequest);
    }
}
