namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllStrategyCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllStrategyCardsQuery, ItemListDto<StrategyCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<StrategyCardDto>> Handle(GetAllStrategyCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var strategyCards = await _cardsRepository.GetAllStrategyCards(cancellationToken);

        var strategyCardsDto = _mapper.Map<List<StrategyCardDto>>(strategyCards);

        return new ItemListDto<StrategyCardDto>(strategyCardsDto);
    }
}
