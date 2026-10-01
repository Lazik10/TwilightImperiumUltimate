using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.ApiContracts.NewsArticle;

namespace TwilightImperiumUltimate.Business.Logic.News;

public class DeleteNewsArticleCommandHandler(
    INewsArticleRepository newsArticleRepository)
    : IRequestHandler<DeleteNewsArticleCommand, ApiResponse<DeleteNewsArticleResponse>>
{
    private readonly INewsArticleRepository _newsArticleRepository = newsArticleRepository;

    public async Task<ApiResponse<DeleteNewsArticleResponse>> Handle(DeleteNewsArticleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deleted = await _newsArticleRepository.DeleteNewsArticle(request.Id, cancellationToken);

        return new ApiResponse<DeleteNewsArticleResponse>() { Success = deleted, Data = new DeleteNewsArticleResponse() { Success = deleted } };
    }
}
