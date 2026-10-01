using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllObjectiveCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllObjectiveCardsQuery, ItemListDto<ObjectiveCardDto>>
{
    private const string CacheKey = "cards:objective:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<ObjectiveCardDto>> Handle(GetAllObjectiveCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<ObjectiveCardDto>? cached) && cached is not null)
            return cached;

        var allObjectiveCards = await _cardsRepository.GetAllObjectiveCards(cancellationToken);

        var allObjectiveCardsDto = _mapper.Map<List<ObjectiveCardDto>>(allObjectiveCards);

        var result = new ItemListDto<ObjectiveCardDto>(allObjectiveCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
