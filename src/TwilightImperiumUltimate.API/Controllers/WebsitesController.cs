namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebsitesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/websites
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<WebsiteDto>>>> GetAllWebsites(CancellationToken cancellationToken)
    {
        var websites = await _mediator.Send(new GetAllWebsitesQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<WebsiteDto>>() { Success = true, Data = websites });
    }
}
