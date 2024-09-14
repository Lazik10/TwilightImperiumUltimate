namespace TwilightImperiumUltimate.Business.Logic.News;

public class GetAllNewsCommandHandler(
    INewsArticleRepository newsArticleRepository,
    IGameStatisticsRepository gameStatisticsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllNewsCommand, ItemListDto<NewsArticleDto>>
{
    private readonly INewsArticleRepository _newsArticleRepository = newsArticleRepository;
    private readonly IGameStatisticsRepository _gameStatisticsRepository = gameStatisticsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<NewsArticleDto>> Handle(GetAllNewsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _gameStatisticsRepository.UpdateWebsiteStatistics(StatisticsType.Visitors, cancellationToken);

        var newsArticles = await _newsArticleRepository.GetAllNewsArticles(cancellationToken);

        var newsArticlesDto = _mapper.Map<List<NewsArticleDto>>(newsArticles);

        return new ItemListDto<NewsArticleDto>(newsArticlesDto);
    }
}
