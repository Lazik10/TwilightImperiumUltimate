using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.News;

public class UpdateNewsArticleCommandHandler(
    INewsArticleRepository newsArticleRepository,
    IMapper mapper)
    : IRequestHandler<UpdateNewsArticleCommand, ApiResponse<NewsArticleDto>>
{
    private readonly INewsArticleRepository _newsArticleRepository = newsArticleRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<NewsArticleDto>> Handle(UpdateNewsArticleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var newsArticle = new NewsArticle
        {
            Id = request.Id,
            Title = request.Title,
            Content = request.Content,
            UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
        };

        var updateSuccessful = await _newsArticleRepository.UpdateNewsArticle(newsArticle, cancellationToken);

        if (!updateSuccessful)
            return new ApiResponse<NewsArticleDto>() { Success = false, Data = null, ProblemDetails = new ProblemDetailsDto() { Title = "Update failed" } };

        var updated = await _newsArticleRepository.GetNewsArticleById(request.Id, cancellationToken);
        var dto = _mapper.Map<NewsArticleDto>(updated);

        return new ApiResponse<NewsArticleDto>() { Success = true, Data = dto };
    }
}
