namespace TwilightImperiumUltimate.Web.AutoMapper;

public class RuleProfile : Profile
{
    public RuleProfile()
    {
        CreateMap<RuleDto, RuleModel>();

        CreateMap<FaqDto, FaqModel>();
        CreateMap<FaqModel, FaqDto>()
            .ConstructUsing(x => new FaqDto(
                x.Id,
                x.ComponentName,
                x.QuestionEnglish,
                x.AnswerEnglish,
                x.QuestionCzech,
                x.AnswerCzech,
                x.FaqStatus));
    }
}
