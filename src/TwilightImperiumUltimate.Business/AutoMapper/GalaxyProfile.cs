using TwilightImperiumUltimate.Contracts.ApiContracts.Maps;
using TwilightImperiumUltimate.Contracts.DTOs.SliceGeneration;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
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
                x.GameVersion));

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

        CreateMap<Map, MapDto>()
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
                x.MapRatings.Count > 0 ? x.MapRatings.Average(x => x.Rating) : 0,
                x.MapRatings.Count));

        CreateMap<MapDto, Map>();

        CreateMap<SliceDraft, SliceDraftDto>()
            .ConstructUsing(x => new SliceDraftDto(
                x.Id,
                x.Name,
                x.EventName,
                x.Description,
                x.SliceNames,
                x.SliceCount,
                x.SliceDraftString,
                x.SliceDraftGeneratorLink,
                x.SliceDraftArchiveLink,
                x.UserName,
                x.UserId,
                x.SliceDraftRatings.Count > 0 ? x.SliceDraftRatings.Average(x => x.Rating) : 0,
                x.SliceDraftRatings.Count));

        CreateMap<SliceDraftDto, SliceDraft>();

        CreateMap<MapRating, MapRatingDto>()
            .ConstructUsing(x => new MapRatingDto(
                x.UserId,
                x.MapId,
                x.Rating));

        CreateMap<SliceDraftRating, SliceDraftRatingDto>()
            .ConstructUsing(x => new SliceDraftRatingDto(
                x.UserId,
                x.SliceDraftId,
                x.Rating));
    }
}
