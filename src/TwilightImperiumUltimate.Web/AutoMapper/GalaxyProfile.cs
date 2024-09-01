namespace TwilightImperiumUltimate.Web.AutoMapper;

public class GalaxyProfile : Profile
{
    public GalaxyProfile()
    {
        CreateMap<PlanetDto, PlanetModel>();
        CreateMap<SystemTileDto, SystemTileModel>();
        CreateMap<WormholeDto, WormholeModel>();
        CreateMap<SliceDto, SliceModel>();

        CreateMap<MapDto, MapModel>();
        CreateMap<MapModel, MapDto>()
            .ConstructUsing(x => new MapDto(
                x.Id,
                x.Name,
                x.EventName,
                x.Description,
                x.MapTemplate,
                x.UserName,
                x.UserId,
                x.TtsString,
                x.MapGeneratorLink,
                x.MapArchiveLink,
                x.Rating,
                x.NumberOfVotes));
    }
}
