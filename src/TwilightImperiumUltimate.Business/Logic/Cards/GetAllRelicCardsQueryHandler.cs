using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllRelicCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllRelicCardsQuery, ItemListDto<RelicCardDto>>
{
    private const string CacheKey = "cards:relic:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<RelicCardDto>> Handle(GetAllRelicCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<RelicCardDto>? cached) && cached is not null)
            return cached;

        var relicCards = await _cardsRepository.GetAllRelicCards(cancellationToken);

        var relicCardsDto = _mapper.Map<List<RelicCardDto>>(relicCards);

        var result = new ItemListDto<RelicCardDto>(relicCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
