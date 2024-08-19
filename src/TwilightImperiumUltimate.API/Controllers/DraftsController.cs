using TwilightImperiumUltimate.Business.Logic.Drafts.Faction;
using TwilightImperiumUltimate.Business.Logic.Drafts.FactionColor;
using TwilightImperiumUltimate.Contracts.ApiContracts.Draft;
using TwilightImperiumUltimate.Contracts.DTOs.Draft;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DraftsController(
    IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // POST: api/drafts/faction
    [Route("faction")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<FactionDraftResultDto>>> GetPlayersWithDraftedFactions(FactionDraftRequest request)
    {
        var factionDraftResult = await _mediator.Send(new DraftFactionsCommand(request));
        return Ok(new ApiResponse<FactionDraftResultDto>() { Success = true, Data = factionDraftResult });
    }

    // POST: api/drafts/color
    [Route("color")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<FactionColorDraftResultDto>>> GetDraftedFactionColors(ColorDraftRequest request)
    {
        var result = await _mediator.Send(new DraftColorsCommand(request));
        return Ok(new ApiResponse<FactionColorDraftResultDto>() { Success = true, Data = result });
    }
}
