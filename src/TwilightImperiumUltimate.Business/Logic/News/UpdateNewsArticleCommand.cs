using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.News;

public class UpdateNewsArticleCommand(int id, string title, string content)
    : IRequest<ApiResponse<NewsArticleDto>>
{
    public int Id { get; } = id;

    public string Title { get; } = title;

    public string Content { get; } = content;
}
