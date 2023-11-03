using TwilightImperiumUltimate.Core.Models.Factions;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft;

public interface IMapDraftService
{
    Task<MapDraftResult> GenerateTilesMapLayout(MapDraftRequest request);
}