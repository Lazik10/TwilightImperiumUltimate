namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<NewsArticleDto>>>> GetAllNews()
    {
        var news = await _mediator.Send(new GetAllNewsCommand());
        return new ApiResponse<ItemListDto<NewsArticleDto>>() { Success = true, Data = news };
    }
}
