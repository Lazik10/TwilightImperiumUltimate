using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllPromissoryNoteCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllPromissoryNoteCardsQuery, ItemListDto<PromissoryNoteCardDto>>
{
    private const string CacheKey = "cards:promissory-note:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<PromissoryNoteCardDto>> Handle(GetAllPromissoryNoteCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<PromissoryNoteCardDto>? cached) && cached is not null)
            return cached;

        var promissoryNoteCards = await _cardsRepository.GetAllPromissoryNoteCards(cancellationToken);

        var promissoryNoteCardsDto = _mapper.Map<List<PromissoryNoteCardDto>>(promissoryNoteCards);

        var result = new ItemListDto<PromissoryNoteCardDto>(promissoryNoteCardsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
