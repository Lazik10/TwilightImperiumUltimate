using TwilightImperiumUltimate.Core.Entities.Rules;
using TiRule = TwilightImperiumUltimate.Core.Entities.Rules.Rule;

namespace TwilightImperiumUltimate.Business.AutoMapper;

internal class RuleProfile : Profile
{
    public RuleProfile()
    {
        CreateMap<TiRule, RuleDto>()
            .ConstructUsing(x => new RuleDto(
                x.Id,
                x.RuleCategory,
                x.Content));

        CreateMap<Faq, FaqDto>()
            .ConstructUsing(x => new FaqDto(
                x.Id,
                x.ComponentName,
                x.QuestionEnglish,
                x.AnswerEnglish,
                x.QuestionCzech,
                x.AnswerCzech,
                x.FaqStatus));

        CreateMap<FaqDto, Faq>();
    }
}
