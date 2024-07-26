namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetAllPromissoryNoteCardsQueryHandler(
    ICardRepository cardsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllPromissoryNoteCardsQuery, ItemListDto<PromissoryNoteCardDto>>
{
    private readonly ICardRepository _cardsRepository = cardsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<PromissoryNoteCardDto>> Handle(GetAllPromissoryNoteCardsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var promissoryNoteCards = await _cardsRepository.GetAllPromissoryNoteCards(cancellationToken);

        var promissoryNoteCardsDto = _mapper.Map<List<PromissoryNoteCardDto>>(promissoryNoteCards);

        return new ItemListDto<PromissoryNoteCardDto>(promissoryNoteCardsDto);
    }
}
