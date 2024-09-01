using TwilightImperiumUltimate.Business.Logic.MapArchive;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/map-archive")]
[ApiController]
public class MapArchiveController(
    IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/map-archive/maps
    [Route("maps")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<MapDto>>>> GetAllMaps()
    {
        var maps = await _mediator.Send(new GetAllMapsQuery());
        return Ok(new ApiResponse<ItemListDto<MapDto>>() { Data = maps });
    }

    // GET: api/map-archive/maps
    [Route("maps/{id:int}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<MapDto>>> GetSpecificMap(int id)
    {
        var response = await _mediator.Send(new GetMapByIdQuery(id));
        return Ok(response);
    }

    // POST: api/map-archive/add-map
    [Route("add-map")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<MapDto>>> AddMap(MapDto map)
    {
        var result = await _mediator.Send(new AddNewMapCommand(map));
        return Ok(new ApiResponse<MapDto>() { Success = result });
    }
}
