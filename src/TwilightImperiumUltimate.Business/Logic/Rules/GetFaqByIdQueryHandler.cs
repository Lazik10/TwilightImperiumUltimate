namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class GetFaqByIdQueryHandler(
    IFaqRepository faqRepository,
    IMapper mapper)
    : IRequestHandler<GetFaqByIdQuery, FaqDto>
{
    private readonly IFaqRepository _faqRepository = faqRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<FaqDto> Handle(GetFaqByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var faq = await _faqRepository.GetFaqById(request.FaqId, cancellationToken);
        return _mapper.Map<FaqDto>(faq);
    }
}
