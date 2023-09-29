using MediatR;
using TwilightImperiumUltimate.API.Models.Factions;
using TwilightImperiumUltimate.Core.Entities.Factions;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionDraft;

internal class DraftFactionsCommandHandler : IRequestHandler<DraftFactionsCommand, List<DraftResult>>
{
    private readonly IFactionDraftService _factionDraftService;

    public DraftFactionsCommandHandler(IFactionDraftService factionDraftService)
    {
        _factionDraftService = factionDraftService;
    }

    public Task<List<DraftResult>> Handle(DraftFactionsCommand command, CancellationToken cancellationToken)
    {
        return _factionDraftService.DraftFactionsAsync(command.FactionDraftRequest);
    }
}
