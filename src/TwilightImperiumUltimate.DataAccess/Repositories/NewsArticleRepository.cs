namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class NewsArticleRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : INewsArticleRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<(List<NewsArticle> Items, int TotalCount)> GetNewsArticlesPage(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var query = dbContext.NewsArticles
            .Include(n => n.User)
            .OrderByDescending(x => x.Id);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
