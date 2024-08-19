namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllExplorationCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllExplorationCardsQuery, ItemListDto<ExplorationCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<ExplorationCardDto>> Handle(GetAllExplorationCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var explorationCards = await _cardsRepository.GetAllExplorationCards(cancellationToken);

        var explorationCardsDto = _mapper.Map<List<ExplorationCardDto>>(explorationCards);

        return new ItemListDto<ExplorationCardDto>(explorationCardsDto);
    }
}
