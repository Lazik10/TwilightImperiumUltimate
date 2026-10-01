namespace TwilightImperiumUltimate.Contracts.ApiContracts.NewsArticle;

public class CreateNewsArticleRequest
{
    public required string Title { get; set; }

    public required string Content { get; set; }
}
