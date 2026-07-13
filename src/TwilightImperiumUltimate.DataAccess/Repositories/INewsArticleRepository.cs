namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface INewsArticleRepository
{
    Task<(List<NewsArticle> Items, int TotalCount)> GetNewsArticlesPage(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<NewsArticle?> GetNewsArticleById(int id, CancellationToken cancellationToken);

    Task<NewsArticle> CreateNewsArticle(NewsArticle newsArticle, CancellationToken cancellationToken);

    Task<bool> UpdateNewsArticle(NewsArticle newsArticle, CancellationToken cancellationToken);

    Task<bool> DeleteNewsArticle(int id, CancellationToken cancellationToken);
}
