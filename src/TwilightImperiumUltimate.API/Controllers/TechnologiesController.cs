namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TechnologiesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<TechnologyDto>>>> GetAllNews()
    {
        var result = await _mediator.Send(new GetAllTechnologiesCommand());
        return Ok(new ApiResponse<ItemListDto<TechnologyDto>>() { Success = true, Data = result });
    }
}
