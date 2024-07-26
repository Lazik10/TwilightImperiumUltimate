using TwilightImperiumUltimate.Web.AutoMapper.Converters;

namespace TwilightImperiumUltimate.Web.AutoMapper;

public class GalaxyProfile : Profile
{
    public GalaxyProfile()
    {
        CreateMap<PlanetDto, PlanetModel>();
        CreateMap<SystemTileDto, SystemTileModel>();

/*        CreateMap<Dictionary<int, SystemTileDto>, Dictionary<int, SystemTileModel>>()
            .ConvertUsing<SystemTileDictionaryConverter>();*/
    }
}
