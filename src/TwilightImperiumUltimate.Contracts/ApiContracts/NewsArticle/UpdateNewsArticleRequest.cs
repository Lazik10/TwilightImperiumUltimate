namespace TwilightImperiumUltimate.Contracts.ApiContracts.NewsArticle;

public class UpdateNewsArticleRequest
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }
}
