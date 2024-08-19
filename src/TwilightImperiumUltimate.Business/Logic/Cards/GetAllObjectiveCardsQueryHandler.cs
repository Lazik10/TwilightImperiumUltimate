namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllObjectiveCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllObjectiveCardsQuery, ItemListDto<ObjectiveCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<ObjectiveCardDto>> Handle(GetAllObjectiveCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var allObjectiveCards = await _cardsRepository.GetAllObjectiveCards(cancellationToken);

        var allObjectiveCardsDto = _mapper.Map<List<ObjectiveCardDto>>(allObjectiveCards);

        return new ItemListDto<ObjectiveCardDto>(allObjectiveCardsDto);
    }
}
