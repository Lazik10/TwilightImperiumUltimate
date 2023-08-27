using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.API.Cards;

[Route("api/[controller]")]
[ApiController]
public class CardsController : ControllerBase
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context;

    public CardsController(IDbContextFactory<TwilightImperiumDbContext> context)
    {
        _context = context;
    }

    // GET: api/cards/secret
    [Route("secret")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjectiveCard>>> GetAllSecretCards()
    {
        using var context = _context.CreateDbContext();

        return await context.ObjectivesCards
            .Where(x => x.ObjectiveCardType == ObjectiveCardType.Secret)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    // GET: api/cards/stageoneobjective
    [Route("stageoneobjective")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjectiveCard>>> GetAllStageOneObjectiveCards()
    {
        using var context = _context.CreateDbContext();

        return await context.ObjectivesCards
            .Where(x => x.ObjectiveCardType == ObjectiveCardType.StageOne)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    // GET: api/cards/stagetwoobjective
    [Route("stagetwoobjective")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjectiveCard>>> GetAllStageTwoObjectiveCards()
    {
        using var context = _context.CreateDbContext();

        return await context.ObjectivesCards
            .Where(x => x.ObjectiveCardType == ObjectiveCardType.StageTwo)
            .ToListAsync()
            .ConfigureAwait(false);
    }
}
