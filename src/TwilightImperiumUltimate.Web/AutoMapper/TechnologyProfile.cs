namespace TwilightImperiumUltimate.Web.AutoMapper;

public class TechnologyProfile : Profile
{
    public TechnologyProfile()
    {
        CreateMap<TechnologyDto, TechnologyModel>();
    }
}
