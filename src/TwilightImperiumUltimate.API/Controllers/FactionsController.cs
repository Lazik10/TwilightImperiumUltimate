namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FactionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<FactionDto>>>> GetAllFactionsInfo(CancellationToken ct)
    {
        var factions = await _mediator.Send(new GetAllFactionsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<FactionDto>>() { Success = true, Data = factions });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IApiResponse<FactionDto>>> GetFactionByFactionId(int id, CancellationToken ct)
    {
        var faction = await _mediator.Send(new GetFactionByIdQuery(id), ct);
        return Ok(new ApiResponse<FactionDto>() { Success = true, Data = faction });
    }
}
