using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.API.DTOs.Rules;
using TwilightImperiumUltimate.Business.Rules;

namespace TwilightImperiumUltimate.API.API.Rules;

[Route("api/[controller]")]
[ApiController]
public class RulesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<List<RuleDto>> GetAllRules()
    {
        var rules = await mediator.Send(new GetAllRulesCommand());

        return rules.Select(x => new RuleDto
        {
            RuleCategory = x.RuleCategory,
            Content = x.Content,
        }).ToList();
    }
}
