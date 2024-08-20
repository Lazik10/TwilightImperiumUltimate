namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class UpdateFaqCommand(
    FaqDto faqDto)
    : IRequest<FaqDto>
{
    public FaqDto Faq { get; set; } = faqDto;
}
