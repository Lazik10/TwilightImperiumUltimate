using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

public class MapGeneratorProfile : Profile
{
    public MapGeneratorProfile()
    {
        CreateMap<GeneratedMapLayout, GeneratedMapLayoutDto>();
    }
}
