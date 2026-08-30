using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllFlagshipCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllFlagshipCardsQuery, ItemListDto<FlagshipCardDto>>
{
    private const string CacheKey = "cards:flagship:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<FlagshipCardDto>> Handle(GetAllFlagshipCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<FlagshipCardDto>? cached) && cached is not null)
            return cached;

        var flagshipCards = await _cardsRepository.GetAllFlagshipCards(cancellationToken);

        var flagshipCardsDto = _mapper.Map<List<FlagshipCardDto>>(flagshipCards);

        var result = new ItemListDto<FlagshipCardDto>(flagshipCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
