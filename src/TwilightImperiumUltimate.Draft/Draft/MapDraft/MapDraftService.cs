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

    public Task<MapDraftResult> GenerateMapLayout(MapDraftRequest request)
    {
        var result = new MapDraftResult
        {
            MapTilePositions = Enumerable.Range(0, 81).ToDictionary(x => x, x => x),
        };

        return Task.FromResult(result);
    }

    public async Task<Dictionary<int, SystemTile>> GenerateTilesMapLayout(MapDraftRequest request)
    {
        var mapBuilder = await _mapBuilderProvider.GetMapBuilder(request.MapTemplate);
        var galaxy = await mapBuilder.GenerateGalaxy();
        return galaxy;
    }
}
