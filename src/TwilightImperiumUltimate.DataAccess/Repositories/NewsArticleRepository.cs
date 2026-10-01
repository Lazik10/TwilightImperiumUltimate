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

    public async Task<NewsArticle?> GetNewsArticleById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.NewsArticles
            .Include(n => n.User)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<NewsArticle> CreateNewsArticle(NewsArticle newsArticle, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        dbContext.NewsArticles.Add(newsArticle);
        await dbContext.SaveChangesAsync(cancellationToken);
        await dbContext.Entry(newsArticle).Reference(n => n.User).LoadAsync(cancellationToken);

        return newsArticle;
    }

    public async Task<bool> UpdateNewsArticle(NewsArticle newsArticle, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var existing = await dbContext.NewsArticles.FirstOrDefaultAsync(n => n.Id == newsArticle.Id, cancellationToken);

        if (existing is null)
            return false;

        existing.Title = newsArticle.Title;
        existing.Content = newsArticle.Content;
        existing.UpdatedAt = newsArticle.UpdatedAt;

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteNewsArticle(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var existing = await dbContext.NewsArticles.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

        if (existing is null)
            return false;

        dbContext.NewsArticles.Remove(existing);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
