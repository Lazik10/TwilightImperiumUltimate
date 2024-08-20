using TwilightImperiumUltimate.Core.Entities.Rules;

namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class UpdateFaqCommandHandler(
    IFaqRepository faqRepository,
    IMapper mapper)
    : IRequestHandler<UpdateFaqCommand, FaqDto>
{
    private readonly IFaqRepository _faqRepository = faqRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<FaqDto> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
    {
        var newFaq = _mapper.Map<Faq>(request.Faq);
        var updatedFaq = await _faqRepository.UpdateFaq(newFaq, cancellationToken);
        return _mapper.Map<FaqDto>(updatedFaq);
    }
}
