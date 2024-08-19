namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllFrontierCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllFrontierCardsQuery, ItemListDto<FrontierCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<FrontierCardDto>> Handle(GetAllFrontierCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var frontierCards = await _cardsRepository.GetAllFrontierCards(cancellationToken);

        var frontierCardsDto = _mapper.Map<List<FrontierCardDto>>(frontierCards);

        return new ItemListDto<FrontierCardDto>(frontierCardsDto);
    }
}
