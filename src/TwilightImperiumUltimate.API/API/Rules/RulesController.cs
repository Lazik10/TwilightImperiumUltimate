using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.API.DTOs.Rules;
using TwilightImperiumUltimate.Business.Rules;

namespace TwilightImperiumUltimate.API.API.Rules;

[Route("api/[controller]")]
[ApiController]
public class RulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<List<RuleDto>> GetAllNews()
    {
        var rules = await _mediator.Send(new GetAllRulesCommand());

        return rules.Select(x => new RuleDto
        {
            RuleCategory = x.RuleCategory,
            Content = x.Content,
            Language = x.Language,
        }).ToList();
    }
}
