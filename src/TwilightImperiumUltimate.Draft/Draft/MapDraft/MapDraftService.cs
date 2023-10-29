using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Core.Models.Factions;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft;

public class MapDraftService : IMapDraftService
{
    private readonly ILogger<MapDraftService> _logger;

    public MapDraftService(ILogger<MapDraftService> logger)
    {
        _logger = logger;
    }

    public Task<MapDraftResult> GenerateMapLayout(MapDraftRequest request)
    {
        var result = new MapDraftResult
        {
            MapTilePositions = Enumerable.Range(0, 81).ToDictionary(x => x, x => x),
        };

        return Task.FromResult(result);
    }
}
