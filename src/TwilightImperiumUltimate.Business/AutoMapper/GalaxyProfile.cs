using TwilightImperiumUltimate.Contracts.DTOs.SliceGeneration;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

public class GalaxyProfile : Profile
{
    public GalaxyProfile()
    {
        CreateMap<Planet, PlanetDto>()
            .ConstructUsing(x => new PlanetDto(
                x.Id,
                x.PlanetName,
                x.Resources,
                x.Influence,
                x.IsLegendary,
                x.TechnologySkip,
                x.PlanetTrait,
                x.GameVersion));

        CreateMap<SystemTile, SystemTileDto>()
            .ConstructUsing(x => new SystemTileDto(
                x.Id,
                x.SystemTileName,
                x.SystemTileCode,
                x.TileCategory,
                x.Resources,
                x.Influence,
                x.HasPlanets,
                x.FactionName,
                x.Planets.Select(p => new PlanetDto(
                    p.Id,
                    p.PlanetName,
                    p.Resources,
                    p.Influence,
                    p.IsLegendary,
                    p.TechnologySkip,
                    p.PlanetTrait,
                    p.GameVersion)).ToList(),
                x.Wormholes.Select(w => new WormholeDto(
                    w.Id,
                    w.WormholeName,
                    w.SystemTileName,
                    w.GameVersion)).ToList(),
                x.Anomaly,
                x.GameVersion));

        CreateMap<Wormhole, WormholeDto>()
            .ConstructUsing(x => new WormholeDto(
                x.Id,
                x.WormholeName,
                x.SystemTileName,
                x.GameVersion)
            );

        CreateMap<Slice, SliceDto>()
            .ConstructUsing(x => new SliceDto(
                x.Id,
                x.DraftedSystemTiles.Select(s => new SystemTileDto(
                    s.Id,
                    s.SystemTileName,
                    s.SystemTileCode,
                    s.TileCategory,
                    s.Resources,
                    s.Influence,
                    s.HasPlanets,
                    s.FactionName,
                    s.Planets.Select(p => new PlanetDto(
                        p.Id,
                        p.PlanetName,
                        p.Resources,
                        p.Influence,
                        p.IsLegendary,
                        p.TechnologySkip,
                        p.PlanetTrait,
                        p.GameVersion)).ToList(),
                    s.Wormholes.Select(w => new WormholeDto(
                        w.Id,
                        w.WormholeName,
                        w.SystemTileName,
                        w.GameVersion)).ToList(),
                    s.Anomaly,
                    s.GameVersion)).ToList()));
    }
}
