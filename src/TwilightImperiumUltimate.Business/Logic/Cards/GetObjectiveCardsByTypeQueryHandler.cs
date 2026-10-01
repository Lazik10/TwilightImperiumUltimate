using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetObjectiveCardsByTypeQueryHandler(
    ICardRepository cardRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetObjectiveCardsByTypeQuery, ItemListDto<ObjectiveCardDto>>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ICardRepository _cardRepository = cardRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<ObjectiveCardDto>> Handle(GetObjectiveCardsByTypeQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = $"cards:objective:type:{request.ObjectiveCardType}";

        if (_memoryCache.TryGetValue(cacheKey, out ItemListDto<ObjectiveCardDto>? cached) && cached is not null)
            return cached;

        var objectiveCardsWithSpecificType = await _cardRepository.GetObjectiveCardsWithSpecificType(request.ObjectiveCardType, cancellationToken);

        var publicStageOneObjectiveCardsDto = _mapper.Map<List<ObjectiveCardDto>>(objectiveCardsWithSpecificType);

        var result = new ItemListDto<ObjectiveCardDto>(publicStageOneObjectiveCardsDto);
        _memoryCache.Set(cacheKey, result, CacheDuration);

        return result;
    }
}
