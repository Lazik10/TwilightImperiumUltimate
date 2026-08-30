using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllFrontierCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllFrontierCardsQuery, ItemListDto<FrontierCardDto>>
{
    private const string CacheKey = "cards:frontier:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<FrontierCardDto>> Handle(GetAllFrontierCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<FrontierCardDto>? cached) && cached is not null)
            return cached;

        var frontierCards = await _cardsRepository.GetAllFrontierCards(cancellationToken);

        var frontierCardsDto = _mapper.Map<List<FrontierCardDto>>(frontierCards);

        var result = new ItemListDto<FrontierCardDto>(frontierCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
