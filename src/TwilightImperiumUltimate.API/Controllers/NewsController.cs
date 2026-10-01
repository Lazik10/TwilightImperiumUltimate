using System.Security.Claims;
using TwilightImperiumUltimate.Contracts.ApiContracts.NewsArticle;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IApiResponse<PagedItemListDto<NewsArticleDto>>>> GetAllNews(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 5,
        CancellationToken cancellationToken = default)
    {
        var news = await _mediator.Send(new GetAllNewsCommand(pageNumber, pageSize), cancellationToken);
        return new ApiResponse<PagedItemListDto<NewsArticleDto>>() { Success = true, Data = news };
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IApiResponse<NewsArticleDto>>> CreateNewsArticle(CreateNewsArticleRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var response = await _mediator.Send(new CreateNewsArticleCommand(request.Title, request.Content, userId), cancellationToken);

        if (response.Success)
            return Ok(response);

        return Conflict(response);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IApiResponse<NewsArticleDto>>> UpdateNewsArticle(UpdateNewsArticleRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await _mediator.Send(new UpdateNewsArticleCommand(request.Id, request.Title, request.Content), cancellationToken);

        if (response.Success)
            return Ok(response);

        return Conflict(response);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IApiResponse<DeleteNewsArticleResponse>>> DeleteNewsArticle(DeleteNewsArticleRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await _mediator.Send(new DeleteNewsArticleCommand(request.Id), cancellationToken);

        if (response.Success)
            return Ok(response);

        return NotFound(response);
    }
}
