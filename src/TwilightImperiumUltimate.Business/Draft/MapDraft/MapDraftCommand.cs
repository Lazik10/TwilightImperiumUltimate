using MediatR;
using TwilightImperiumUltimate.Core.Models.Factions;
using TwilightImperiumUltimate.Draft.Draft.MapDraft;

namespace TwilightImperiumUltimate.Business.Draft.MapDraft;

public class MapDraftCommand : IRequest<MapDraftResult>
{
    public MapDraftCommand(MapDraftRequest draftRequest)
    {
        DraftRequest = draftRequest;
    }

    public MapDraftRequest DraftRequest { get; }
}
