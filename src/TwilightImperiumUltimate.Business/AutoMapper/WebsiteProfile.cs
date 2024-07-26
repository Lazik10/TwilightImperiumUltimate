using TwilightImperiumUltimate.Contracts.DTOs.Website;
using TwilightImperiumUltimate.Core.Entities.Website;

namespace TwilightImperiumUltimate.Business.AutoMapper;

public class WebsiteProfile : Profile
{
    public WebsiteProfile()
    {
        CreateMap<Website, WebsiteDto>();
    }
}
