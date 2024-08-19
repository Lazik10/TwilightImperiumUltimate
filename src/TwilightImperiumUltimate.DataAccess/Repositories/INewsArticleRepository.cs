namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface INewsArticleRepository
{
    Task<List<NewsArticle>> GetAllNewsArticles(CancellationToken cancellationToken);
}
