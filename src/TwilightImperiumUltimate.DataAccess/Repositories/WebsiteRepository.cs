using TwilightImperiumUltimate.Core.Entities.Website;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class WebsiteRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IWebsiteRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context; 

    public async Task<List<Website>> GetAllWebsites(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Websites
            .ToListAsync(cancellationToken);
    }
}
