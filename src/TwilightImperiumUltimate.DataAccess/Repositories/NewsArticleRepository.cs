namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class NewsArticleRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : INewsArticleRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<List<NewsArticle>> GetAllNewsArticles(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.NewsArticles
            .OrderByDescending(x => x.Id)
            .Include(n => n.User)
            .ToListAsync(cancellationToken);
    }
}
