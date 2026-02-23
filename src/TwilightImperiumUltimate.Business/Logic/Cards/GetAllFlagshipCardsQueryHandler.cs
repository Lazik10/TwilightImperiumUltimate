namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllFlagshipCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllFlagshipCardsQuery, ItemListDto<FlagshipCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<FlagshipCardDto>> Handle(GetAllFlagshipCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var flagshipCards = await _cardsRepository.GetAllFlagshipCards(cancellationToken);

        var flagshipCardsDto = _mapper.Map<List<FlagshipCardDto>>(flagshipCards);

        return new ItemListDto<FlagshipCardDto>(flagshipCardsDto);
    }
}
