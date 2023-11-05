using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.API.DTOs.Galaxy;
using TwilightImperiumUltimate.Business.Draft.FactionDraft;
using TwilightImperiumUltimate.Business.Draft.MapDraft;
using TwilightImperiumUltimate.Core.Models.Factions;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;
using TwilightImperiumUltimate.Draft.Draft.MapDraft;

namespace TwilightImperiumUltimate.API.API.MapGenerators;

[Route("api/[controller]")]
[ApiController]
public class MapGeneratorsController : ControllerBase
{
    private readonly ILogger<MapGeneratorsController> _logger;
    private readonly IMediator _mediator;

    public MapGeneratorsController(ILogger<MapGeneratorsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    // POST: api/mapgenerators/
    [HttpPost]
    public async Task<MapDraftResultDto> GetGeneratedMap(MapDraftRequest request)
    {
        _logger.LogInformation("Registered faction draft process at {Time}", DateTime.Now.ToShortTimeString());
        var mapDraftresult = await _mediator.Send(new MapDraftCommand(request));

        var result = new MapDraftResultDto();

        foreach (var mapTile in mapDraftresult.MapTiles)
        {
            result.MapTiles.Add(mapTile.Key, new SystemTileDto
            {
                Id = mapTile.Value.Id,
                Name = mapTile.Value.SystemTileName,
                TileCategory = mapTile.Value.TileCategory,
                HasPlanets = mapTile.Value.HasPlanets,
                Influence = mapTile.Value.Influence,
                Resources = mapTile.Value.Resources,
            });
        }

        return result;
    }
}
