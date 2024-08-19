namespace TwilightImperiumUltimate.Web.AutoMapper;

public class WebsiteProfile : Profile
{
    public WebsiteProfile()
    {
        CreateMap<WebsiteDto, WebsiteModel>();
    }
}
