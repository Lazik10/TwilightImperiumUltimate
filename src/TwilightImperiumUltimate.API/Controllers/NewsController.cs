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
}
