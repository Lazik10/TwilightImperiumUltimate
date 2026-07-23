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

    public async Task<Website> CreateWebsite(Website website, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        dbContext.Websites.Add(website);
        await dbContext.SaveChangesAsync(cancellationToken);

        return website;
    }

    public async Task<bool> DeleteWebsite(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var existing = await dbContext.Websites.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if (existing is null)
            return false;

        dbContext.Websites.Remove(existing);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
