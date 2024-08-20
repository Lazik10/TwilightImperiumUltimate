namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class InsertNewFaqCommand(
    FaqDto faq)
    : IRequest<FaqDto>
{
    public FaqDto Faq { get; set; } = faq;
}
