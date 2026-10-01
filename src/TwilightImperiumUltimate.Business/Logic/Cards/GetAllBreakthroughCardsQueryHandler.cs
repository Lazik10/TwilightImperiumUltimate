using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllBreakthroughCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllBreakthroughCardsQuery, ItemListDto<BreakthroughCardDto>>
{
    private const string CacheKey = "cards:breakthrough:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<BreakthroughCardDto>> Handle(GetAllBreakthroughCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<BreakthroughCardDto>? cached) && cached is not null)
            return cached;

        var breakthroughCards = await _cardsRepository.GetAllBreakthroughCards(cancellationToken);

        var breakthroughCardsDto = _mapper.Map<List<BreakthroughCardDto>>(breakthroughCards);

        var result = new ItemListDto<BreakthroughCardDto>(breakthroughCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
