namespace TwilightImperiumUltimate.Business.Logic.News;

public class GetAllNewsCommandHandler(
    INewsArticleRepository newsArticleRepository,
    IGameStatisticsRepository gameStatisticsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllNewsCommand, PagedItemListDto<NewsArticleDto>>
{
    private readonly INewsArticleRepository _newsArticleRepository = newsArticleRepository;
    private readonly IGameStatisticsRepository _gameStatisticsRepository = gameStatisticsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedItemListDto<NewsArticleDto>> Handle(GetAllNewsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Only count a visitor hit for the first page -- paging through older news on the same
        // visit shouldn't inflate the visitor statistic.
        if (request.PageNumber == 1)
            await _gameStatisticsRepository.UpdateWebsiteStatistics(StatisticsType.Visitors, cancellationToken);

        var (newsArticles, totalCount) = await _newsArticleRepository.GetNewsArticlesPage(request.PageNumber, request.PageSize, cancellationToken);

        var newsArticlesDto = _mapper.Map<List<NewsArticleDto>>(newsArticles);

        return new PagedItemListDto<NewsArticleDto>(newsArticlesDto, totalCount);
    }
}
