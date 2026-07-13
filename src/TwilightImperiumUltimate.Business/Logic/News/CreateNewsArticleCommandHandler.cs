using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.News;

public class CreateNewsArticleCommandHandler(
    INewsArticleRepository newsArticleRepository,
    IMapper mapper)
    : IRequestHandler<CreateNewsArticleCommand, ApiResponse<NewsArticleDto>>
{
    private readonly INewsArticleRepository _newsArticleRepository = newsArticleRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<NewsArticleDto>> Handle(CreateNewsArticleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var newsArticle = new NewsArticle
        {
            Title = request.Title,
            Content = request.Content,
            UserId = request.UserId,
            CreatedAt = today,
            UpdatedAt = today,
        };

        var created = await _newsArticleRepository.CreateNewsArticle(newsArticle, cancellationToken);
        var dto = _mapper.Map<NewsArticleDto>(created);

        return new ApiResponse<NewsArticleDto>() { Success = true, Data = dto };
    }
}
