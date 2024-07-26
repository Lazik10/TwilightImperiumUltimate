namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class FactionRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IFactionRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<List<Faction>> GetAllFactions(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return dbContext.Factions
            .Include(x => x.FactionUnits)
            .ThenInclude(fu => fu.Unit)
            .Include(x => x.FactionTechnologies)
            .ThenInclude(ft => ft.Technology)
            .AsEnumerable()
            .OrderBy(x => x.FactionName)
            .ToList();
    }

    public async Task<Faction> GetFactionById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Factions
            .Include(x => x.FactionUnits)
            .ThenInclude(fu => fu.Unit)
            .Include(x => x.FactionTechnologies)
            .ThenInclude(ft => ft.Technology)
            .SingleAsync(x => x.FactionName == (FactionName)id, cancellationToken);
    }
}
