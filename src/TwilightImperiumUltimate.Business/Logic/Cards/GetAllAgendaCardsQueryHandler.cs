using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllAgendaCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllAgendaCardsQuery, ItemListDto<AgendaCardDto>>
{
    private const string CacheKey = "cards:agenda:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<AgendaCardDto>> Handle(GetAllAgendaCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<AgendaCardDto>? cached) && cached is not null)
            return cached;

        var agendaCards = await _cardsRepository.GetAllAgendaCards(cancellationToken);

        var agendaCardsDto = _mapper.Map<List<AgendaCardDto>>(agendaCards);

        var result = new ItemListDto<AgendaCardDto>(agendaCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
