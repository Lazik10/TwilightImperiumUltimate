namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface INewsArticleRepository
{
    Task<(List<NewsArticle> Items, int TotalCount)> GetNewsArticlesPage(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
