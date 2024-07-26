namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/map-generators/")]
[ApiController]
public class MapGeneratorsController(
    IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // POST: api/map-generators/generate-map
    [Route("generate-map")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<GeneratedMapLayoutDto>>> GetGeneratedMap(GenerateMapRequest request)
    {
        var mapLayout = await _mediator.Send(new GenerateMapCommand(request));
        return Ok(new ApiResponse<GeneratedMapLayoutDto>() { Success = true, Data = mapLayout });
    }
}
