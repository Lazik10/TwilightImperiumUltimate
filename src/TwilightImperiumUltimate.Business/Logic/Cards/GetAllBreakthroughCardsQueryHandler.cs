namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllBreakthroughCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllBreakthroughCardsQuery, ItemListDto<BreakthroughCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<BreakthroughCardDto>> Handle(GetAllBreakthroughCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var breakthroughCards = await _cardsRepository.GetAllBreakthroughCards(cancellationToken);

        var breakthroughCardsDto = _mapper.Map<List<BreakthroughCardDto>>(breakthroughCards);

        return new ItemListDto<BreakthroughCardDto>(breakthroughCardsDto);
    }
}
