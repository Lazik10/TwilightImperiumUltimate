using TwilightImperiumUltimate.Core.Entities.Rules;

namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class InsertNewFaqCommandHandler(
    IFaqRepository faqRepository,
    IMapper mapper)
    : IRequestHandler<InsertNewFaqCommand, FaqDto>
{
    private readonly IFaqRepository _faqRepository = faqRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<FaqDto> Handle(InsertNewFaqCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var faq = _mapper.Map<Faq>(request.Faq);
        await _faqRepository.AddNewFaq(faq, cancellationToken);
        return _mapper.Map<FaqDto>(faq);
    }
}
