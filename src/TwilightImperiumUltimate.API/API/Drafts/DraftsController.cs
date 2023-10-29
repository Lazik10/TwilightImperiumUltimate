using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.API.Models.Factions;
using TwilightImperiumUltimate.Business.Draft.FactionColorDraft;
using TwilightImperiumUltimate.Business.Draft.FactionDraft;
using TwilightImperiumUltimate.Core.Models.Factions;
using TwilightImperiumUltimate.Draft.Draft.ColorDraft;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;

namespace TwilightImperiumUltimate.API.API.Drafts;

[Route("api/[controller]")]
[ApiController]
public class DraftsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DraftsController> _logger;

    public DraftsController(IMediator mediator, ILogger<DraftsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // POST: api/drafts/factions
    [Route("factions")]
    [HttpPost]
    public async Task<ActionResult<List<FactionDraftResult>>> GetPlayersWithDraftedFactions(FactionDraftRequest request)
    {
        _logger.LogInformation("Registered faction draft process at {Time}", DateTime.Now.ToShortTimeString());
        var result = await _mediator.Send(new FactionsDraftCommand(request));

        return Ok(result);
    }

    // POST: api/drafts/colors
    [Route("colors")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<FactionColorDraftResult>>> GetDraftedFactionColors(ColorDraftRequest request)
    {
        _logger.LogInformation("Registered color draft process at {Time}", DateTime.Now.ToShortTimeString());
        var result = await _mediator.Send(new ColorsDraftCommand(request));

        return Ok(result);
    }
}
