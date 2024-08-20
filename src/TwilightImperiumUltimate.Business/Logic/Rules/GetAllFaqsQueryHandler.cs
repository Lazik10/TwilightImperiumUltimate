namespace TwilightImperiumUltimate.Business.Logic.Rules;

internal class GetAllFaqsQueryHandler(
    IFaqRepository faqRepository,
    IMapper mapper)
    : IRequestHandler<GetAllFaqsQuery, ItemListDto<FaqDto>>
{
    private readonly IFaqRepository _faqRepository = faqRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<FaqDto>> Handle(GetAllFaqsQuery request, CancellationToken cancellationToken)
    {
        var faqs = await _faqRepository.GetAllFaqs(cancellationToken);

        var faqsDto = _mapper.Map<List<FaqDto>>(faqs);

        return new ItemListDto<FaqDto>(faqsDto);
    }
}
