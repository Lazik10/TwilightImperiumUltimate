using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class SpecificMapBuilderProvider : ISpecificMapBuilderProvider
{
    public Task<IMapBuilder> GetMapBuilderForSpecificTemplate(MapTemplate mapTemplate)
    {
        IMapBuilder map = mapTemplate switch
        {
            MapTemplate.CustomMap => new CustomMapBuilder(),
            MapTemplate.ThreePlayersSmallMap => new ThreePlayersSmallMapBuilder(),
            MapTemplate.ThreePlayersSmallAlternateMap => new ThreePlayersSmallAlternateMapBuilder(),
            MapTemplate.ThreePlayersTriangleMap => new ThreePlayersTriangleMapBuilder(),
            MapTemplate.ThreePlayersTriangleNarrowMap => new ThreePlayersTriangleNarrowMapBuilder(),
            MapTemplate.ThreePlayersSnowflakeMap => new ThreePlayersSnowflakeMapBuilder(),
            MapTemplate.ThreePlayersTridentMap => new ThreePlayersTridentMapBuilder(),
            MapTemplate.ThreePlayersMantaRayMap => new ThreePlayersMantaRayMapBuilder(),
            MapTemplate.FourPlayersMediumMap => new FourPlayersMediumMapBuilder(),
            MapTemplate.SixPlayersMediumMap => new SixPlayersMediumMapBuilder(),
            MapTemplate.SixPlayersLargeMap => new SixPlayersLargeMapBuilder(),
            MapTemplate.EightPlayersLargeMap => new EightPlayersLargeMapBuilder(),
            _ => throw new ArgumentOutOfRangeException(nameof(mapTemplate), mapTemplate, null),
        };

        return Task.FromResult(map);
    }
}
