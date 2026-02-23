namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllSpecialComponentCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllSpecialComponentCardsQuery, ItemListDto<SpecialComponentCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<SpecialComponentCardDto>> Handle(GetAllSpecialComponentCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var specialComponentCards = await _cardsRepository.GetAllSpecialComponentCards(cancellationToken);

        var specialComponentCardsDto = _mapper.Map<List<SpecialComponentCardDto>>(specialComponentCards);

        return new ItemListDto<SpecialComponentCardDto>(specialComponentCardsDto);
    }
}
