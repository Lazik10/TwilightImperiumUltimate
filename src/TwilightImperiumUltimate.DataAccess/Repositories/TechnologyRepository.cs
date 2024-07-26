namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class TechnologyRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : ITechnologyRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<List<Technology>> GetAllTechnologies(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Technologies
            .ToListAsync(cancellationToken);
    }
}
