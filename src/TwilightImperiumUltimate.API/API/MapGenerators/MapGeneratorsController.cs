using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.Business.Draft.MapDraft;
using TwilightImperiumUltimate.Core.Models.Factions;
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
    public async Task<ActionResult<Dictionary<int, int>>> GetPlayersWithDraftedFactions(MapDraftRequest request)
    {
        _logger.LogInformation("Registered faction draft process at {Time}", DateTime.Now.ToShortTimeString());
        var result = await _mediator.Send(new MapDraftCommand(request));

        return Ok(result.MapTilePositions);
    }
}
