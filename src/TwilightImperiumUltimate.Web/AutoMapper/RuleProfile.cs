namespace TwilightImperiumUltimate.Web.AutoMapper;

public class RuleProfile : Profile
{
    public RuleProfile()
    {
        CreateMap<RuleDto, RuleModel>();
    }
}
