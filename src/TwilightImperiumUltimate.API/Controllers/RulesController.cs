namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RulesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ApiResponse<ItemListDto<RuleDto>>> GetAllRules()
    {
        var rules = await mediator.Send(new GetAllRulesCommand());
        return new ApiResponse<ItemListDto<RuleDto>>() { Success = true, Data = rules };
    }
}
