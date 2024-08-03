using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

public class MapGeneratorProfile : Profile
{
    public MapGeneratorProfile()
    {
/*        CreateMap<Hex, HexDto>()
            .ForMember(d => d.SystemTile, opt => opt.Ignore())
            .ConstructUsing((x, context) =>
            {
                return new HexDto() { X = x.X, Y = x.Y, Name = x.Name, SystemTile = context.Mapper.Map<SystemTileDto>(x.SystemTile) };
            });*/

        // This is a custom mapping that is not supported by AutoMapper out of the box ignore the MapLayout property and map it manually
/*        CreateMap<GeneratedMapLayout, GeneratedMapLayoutDto>()
            .ForMember(d => d.MapLayout, opt => opt.Ignore())
            .ConstructUsing((g, context) =>
            {
                return new GeneratedMapLayoutDto(
                    g.MapLayout.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new HexDto(kvp.Value.X, kvp.Value.Y, context.Mapper.Map<SystemTileDto>(kvp.Value.SystemTile))));
            });*/
    }
}
