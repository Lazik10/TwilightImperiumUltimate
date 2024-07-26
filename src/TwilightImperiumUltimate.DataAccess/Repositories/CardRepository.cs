namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class CardRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : ICardRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<List<ActionCard>> GetAllActionCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.ActionCards.ToListAsync(ct);
    }

    public async Task<List<AgendaCard>> GetAllAgendaCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.AgendaCards.ToListAsync(ct);
    }

    public async Task<List<ExplorationCard>> GetAllExplorationCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.ExplorationCards.ToListAsync(ct);
    }

    public async Task<List<FrontierCard>> GetAllFrontierCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.FrontierCards.ToListAsync(ct);
    }

    public async Task<List<ObjectiveCard>> GetAllObjectiveCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.ObjectiveCards.ToListAsync(ct);
    }

    public async Task<List<ObjectiveCard>> GetObjectiveCardsWithSpecificType(ObjectiveCardType objectiveCardType, CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.ObjectiveCards
            .Where(x => x.ObjectiveCardType == objectiveCardType)
            .ToListAsync(ct);
    }

    public async Task<List<PromissoryNoteCard>> GetAllPromissoryNoteCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.PromissoryNoteCards.ToListAsync(ct);
    }

    public async Task<List<RelicCard>> GetAllRelicCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return await dbContext.RelicCards.ToListAsync(ct);
    }

    public async Task<List<StrategyCard>> GetAllStrategyCards(CancellationToken ct)
    {
        await using var dbContext = await _context.CreateDbContextAsync(ct);
        return dbContext.StrategyCards
            .AsEnumerable()
            .OrderBy(x => x.InitiativeOrder)
            .ToList();
    }
}
