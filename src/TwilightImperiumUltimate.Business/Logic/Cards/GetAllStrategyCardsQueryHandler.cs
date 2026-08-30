using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllStrategyCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllStrategyCardsQuery, ItemListDto<StrategyCardDto>>
{
    private const string CacheKey = "cards:strategy:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<StrategyCardDto>> Handle(GetAllStrategyCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<StrategyCardDto>? cached) && cached is not null)
            return cached;

        var strategyCards = await _cardsRepository.GetAllStrategyCards(cancellationToken);

        var strategyCardsDto = _mapper.Map<List<StrategyCardDto>>(strategyCards);

        var result = new ItemListDto<StrategyCardDto>(strategyCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
