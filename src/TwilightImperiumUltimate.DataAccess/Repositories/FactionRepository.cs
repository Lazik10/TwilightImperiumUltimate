namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class FactionRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IFactionRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<List<Faction>> GetAllFactions(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Factions
            .AsNoTracking()
            .Include(x => x.FactionUnits)
            .ThenInclude(fu => fu.Unit)
            .Include(x => x.FactionTechnologies)
            .ThenInclude(ft => ft.Technology)
            .OrderBy(x => x.Id)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task<Faction> GetFactionById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Factions
            .AsNoTracking()
            .Include(x => x.FactionUnits)
            .ThenInclude(fu => fu.Unit)
            .Include(x => x.FactionTechnologies)
            .ThenInclude(ft => ft.Technology)
            .AsSplitQuery()
            .SingleAsync(x => x.FactionName == (FactionName)id, cancellationToken);
    }

    public async Task<List<Faction>> GetFactionsByGameVersions(IReadOnlyCollection<GameVersion> gameVersions, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Factions
            .AsNoTracking()
            .Include(x => x.FactionUnits)
            .ThenInclude(fu => fu.Unit)
            .Include(x => x.FactionTechnologies)
            .ThenInclude(ft => ft.Technology)
            .Where(x => gameVersions.Contains(x.GameVersion))
            .OrderBy(x => x.Id)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}
