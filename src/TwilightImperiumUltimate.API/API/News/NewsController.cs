using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.Business.News;
using TwilightImperiumUltimate.Core.Entities.News;

namespace TwilightImperiumUltimate.API.API.News;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<List<NewsArticle>> GetAllNews()
    {
        return await _mediator.Send(new GetAllNewsCommand());
    }
}
