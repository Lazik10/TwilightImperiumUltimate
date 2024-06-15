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
        using var context = await _context.CreateDbContextAsync();

        return await context.ObjectiveCards
            .Where(x => x.ObjectiveCardType == ObjectiveCardType.Secret)
            .ToListAsync();
    }

    // GET: api/cards/stageoneobjective
    [Route("stageoneobjective")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjectiveCard>>> GetAllStageOneObjectiveCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.ObjectiveCards
            .Where(x => x.ObjectiveCardType == ObjectiveCardType.StageOne)
            .ToListAsync();
    }

    // GET: api/cards/stagetwoobjective
    [Route("stagetwoobjective")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjectiveCard>>> GetAllStageTwoObjectiveCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.ObjectiveCards
            .Where(x => x.ObjectiveCardType == ObjectiveCardType.StageTwo)
            .ToListAsync();
    }

    // GET: api/cards/action
    [Route("action")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActionCard>>> GetAllActionCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.ActionCards.ToListAsync();
    }

    // GET: api/cards/agenda
    [Route("agenda")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AgendaCard>>> GetAllAgendaCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.AgendaCards.ToListAsync();
    }

    // GET: api/cards/frontier
    [Route("frontier")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FrontierCard>>> GetAllFrontierCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.FrontierCards.ToListAsync();
    }

    // GET: api/cards/relic
    [Route("relic")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RelicCard>>> GetAllRelicCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.RelicCards.ToListAsync();
    }

    // GET: api/cards/strategy
    [Route("strategy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StrategyCard>>> GetAllStrategyCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.StrategyCards
            .OrderBy(x => x.InitiativeOrder)
            .ToListAsync();
    }

    // GET: api/cards/exploration
    [Route("exploration")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExplorationCard>>> GetAllExplorationCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.ExplorationCards.ToListAsync();
    }

    // GET: api/cards/promissorynote
    [Route("promissorynote")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PromissaryNoteCard>>> GetAllPromissaryCards()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.PromissaryNoteCards.ToListAsync();
    }
}
