namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class GetFaqByIdQuery(int faqId) : IRequest<FaqDto>
{
    public int FaqId { get; } = faqId;
}
