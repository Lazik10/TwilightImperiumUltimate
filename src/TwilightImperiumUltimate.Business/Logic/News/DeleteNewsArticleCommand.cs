using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.ApiContracts.NewsArticle;

namespace TwilightImperiumUltimate.Business.Logic.News;

public class DeleteNewsArticleCommand(int id)
    : IRequest<ApiResponse<DeleteNewsArticleResponse>>
{
    public int Id { get; } = id;
}
