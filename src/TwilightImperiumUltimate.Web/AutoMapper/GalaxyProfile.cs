namespace TwilightImperiumUltimate.Web.AutoMapper;

public class GalaxyProfile : Profile
{
    public GalaxyProfile()
    {
        CreateMap<PlanetDto, PlanetModel>();
        CreateMap<SystemTileDto, SystemTileModel>();
        CreateMap<WormholeDto, WormholeModel>();
        CreateMap<SliceDto, SliceModel>();
    }
}
