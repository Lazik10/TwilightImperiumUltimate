using MediatR;
using TwilightImperiumUltimate.Draft.Draft.MapDraft;

namespace TwilightImperiumUltimate.Business.Draft.MapDraft;

public class MapDraftCommandHandler : IRequestHandler<MapDraftCommand, MapDraftResult>
{
    private readonly IMapDraftService _mapDraftService;

    public MapDraftCommandHandler(IMapDraftService factionDraftService)
    {
        _mapDraftService = factionDraftService;
    }

    public Task<MapDraftResult> Handle(MapDraftCommand request, CancellationToken cancellationToken)
    {
        return _mapDraftService.GenerateTilesMapLayout(request.DraftRequest);
    }
}
