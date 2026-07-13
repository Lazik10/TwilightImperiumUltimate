using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.News;

public class CreateNewsArticleCommand(string title, string content, string userId)
    : IRequest<ApiResponse<NewsArticleDto>>
{
    public string Title { get; } = title;

    public string Content { get; } = content;

    public string UserId { get; } = userId;
}
