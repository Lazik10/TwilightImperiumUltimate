namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllActionCardsQueryHandler(
    ICardRepository cardRepository,
    IMapper mapper)
    : IRequestHandler<GetAllActionCardsQuery, ItemListDto<ActionCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<ActionCardDto>> Handle(GetAllActionCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var actionCards = await _cardsRepository.GetAllActionCards(cancellationToken);

        var actionCardsDto = _mapper.Map<List<ActionCardDto>>(actionCards);

        return new ItemListDto<ActionCardDto>(actionCardsDto);
    }
}
