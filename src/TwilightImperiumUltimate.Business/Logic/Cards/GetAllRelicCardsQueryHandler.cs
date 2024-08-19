namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllRelicCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllRelicCardsQuery, ItemListDto<RelicCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<RelicCardDto>> Handle(GetAllRelicCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var relicCards = await _cardsRepository.GetAllRelicCards(cancellationToken);

        var relicCardsDto = _mapper.Map<List<RelicCardDto>>(relicCards);

        return new ItemListDto<RelicCardDto>(relicCardsDto);
    }
}
