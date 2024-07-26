namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetObjectiveCardsByTypeQueryHandler(
    ICardRepository cardRepository,
    IMapper mapper)
    : IRequestHandler<GetObjectiveCardsByTypeQuery, ItemListDto<ObjectiveCardDto>>
{
    private readonly ICardRepository _cardRepository = cardRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<ObjectiveCardDto>> Handle(GetObjectiveCardsByTypeQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var objectiveCardsWithSpecificType = await _cardRepository.GetObjectiveCardsWithSpecificType(request.ObjectiveCardType, cancellationToken);

        var publicStageOneObjectiveCardsDto = _mapper.Map<List<ObjectiveCardDto>>(objectiveCardsWithSpecificType);

        return new ItemListDto<ObjectiveCardDto>(publicStageOneObjectiveCardsDto);
    }
}
