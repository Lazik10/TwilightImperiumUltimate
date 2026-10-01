using TwilightImperiumUltimate.Contracts.ApiContracts.Website;

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

    // POST: api/websites
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IApiResponse<WebsiteDto>>> CreateWebsite(CreateWebsiteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await _mediator.Send(
            new CreateWebsiteCommand(request.Title, request.Description, request.WebsitePath, request.ImageData, request.ImageContentType),
            cancellationToken);

        if (response.Success)
            return Ok(response);

        return Conflict(response);
    }

    // DELETE: api/websites
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IApiResponse<DeleteWebsiteResponse>>> DeleteWebsite(DeleteWebsiteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await _mediator.Send(new DeleteWebsiteCommand(request.Id), cancellationToken);

        if (response.Success)
            return Ok(response);

        return NotFound(response);
    }
}
