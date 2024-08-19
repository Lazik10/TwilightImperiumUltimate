namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GalaxyController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Route("system-tiles")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<SystemTileDto>>>> GetAllSystemTiles(CancellationToken cancellationToken)
    {
        var systemTiles = await _mediator.Send(new GetAllSystemTilesQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<SystemTileDto>>() { Success = true, Data = systemTiles });
    }

    [Route("planets")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<PlanetDto>>>> GetAllPlanets()
    {
        var planets = await _mediator.Send(new GetAllPlanetsCommand());
        return Ok(new ApiResponse<ItemListDto<PlanetDto>>() { Success = true, Data = planets });
    }
}
