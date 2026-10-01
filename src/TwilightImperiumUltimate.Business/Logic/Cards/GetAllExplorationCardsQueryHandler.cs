using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllExplorationCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllExplorationCardsQuery, ItemListDto<ExplorationCardDto>>
{
    private const string CacheKey = "cards:exploration:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<ExplorationCardDto>> Handle(GetAllExplorationCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<ExplorationCardDto>? cached) && cached is not null)
            return cached;

        var explorationCards = await _cardsRepository.GetAllExplorationCards(cancellationToken);

        var explorationCardsDto = _mapper.Map<List<ExplorationCardDto>>(explorationCards);

        var result = new ItemListDto<ExplorationCardDto>(explorationCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
