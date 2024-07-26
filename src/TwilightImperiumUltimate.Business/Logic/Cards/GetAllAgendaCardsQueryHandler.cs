namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllAgendaCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllAgendaCardsQuery, ItemListDto<AgendaCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<AgendaCardDto>> Handle(GetAllAgendaCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agendaCards = await _cardsRepository.GetAllAgendaCards(cancellationToken);

        var agendaCardsDto = _mapper.Map<List<AgendaCardDto>>(agendaCards);

        return new ItemListDto<AgendaCardDto>(agendaCardsDto);
    }
}
