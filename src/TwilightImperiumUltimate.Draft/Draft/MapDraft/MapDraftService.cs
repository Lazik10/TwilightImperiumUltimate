using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Core.Models.Factions;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft;

public class MapDraftService : IMapDraftService
{
    private readonly IMapBuilderProvider _mapBuilderProvider;

    public MapDraftService(IMapBuilderProvider mapBuilderProvider)
    {
        _mapBuilderProvider = mapBuilderProvider;
    }

    public async Task<MapDraftResult> GenerateTilesMapLayout(MapDraftRequest request)
    {
        var result = new MapDraftResult();
        var mapBuilder = await _mapBuilderProvider.GetMapBuilder(request.MapTemplate);
        result.MapTiles = await mapBuilder.GenerateGalaxy(request.PreviewMap);
        return result;
    }
}
