using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllSpecialComponentCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllSpecialComponentCardsQuery, ItemListDto<SpecialComponentCardDto>>
{
    private const string CacheKey = "cards:special-component:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<SpecialComponentCardDto>> Handle(GetAllSpecialComponentCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<SpecialComponentCardDto>? cached) && cached is not null)
            return cached;

        var specialComponentCards = await _cardsRepository.GetAllSpecialComponentCards(cancellationToken);

        var specialComponentCardsDto = _mapper.Map<List<SpecialComponentCardDto>>(specialComponentCards);

        var result = new ItemListDto<SpecialComponentCardDto>(specialComponentCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
