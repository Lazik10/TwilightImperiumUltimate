using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllActionCardsQueryHandler(
    ICardRepository cardRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllActionCardsQuery, ItemListDto<ActionCardDto>>
{
    private const string CacheKey = "cards:action:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<ActionCardDto>> Handle(GetAllActionCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<ActionCardDto>? cached) && cached is not null)
            return cached;

        var actionCards = await _cardsRepository.GetAllActionCards(cancellationToken);

        var actionCardsDto = _mapper.Map<List<ActionCardDto>>(actionCards);

        var result = new ItemListDto<ActionCardDto>(actionCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
