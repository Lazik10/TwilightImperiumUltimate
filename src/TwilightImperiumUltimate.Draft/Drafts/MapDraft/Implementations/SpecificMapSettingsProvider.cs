using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class SpecificMapSettingsProvider : ISpecificMapSettingProvider
{
    public Task<IMapSettings> GetMapSettingsForSpecificTemplate(MapTemplate mapTemplate)
    {
        IMapSettings mapSettings = mapTemplate switch
        {
            MapTemplate.CustomMap => new CustomMapSettings(),
            MapTemplate.ThreePlayersSmallMap => new ThreePlayersSmallMapSettings(),
            MapTemplate.ThreePlayersSmallAlternateMap => new ThreePlayersSmallAlternateMapSettings(),
            MapTemplate.ThreePlayersMediumTriangleMap => new ThreePlayersMediumTriangleMapSettings(),
            MapTemplate.ThreePlayersMediumTriangleNarrowMap => new ThreePlayersMediumTriangleNarrowMapSettings(),
            MapTemplate.ThreePlayersMediumSnowflakeMap => new ThreePlayersMediumSnowflakeMapSettings(),
            MapTemplate.ThreePlayersMediumMantaRayMap => new ThreePlayersMediumMantaRayMapSettings(),
            MapTemplate.FourPlayersMediumMap => new FourPlayersMediumMapSettings(),
            MapTemplate.FourPlayersMediumHorizontalMap => new FourPlayersMediumHorizontalMapSettings(),
            MapTemplate.FourPlayersMediumVerticalMap => new FourPlayersMediumVerticalMapSettings(),
            MapTemplate.FourPlayersMediumGapsMap => new FourPlayersMediumGapsMapSettings(),
            MapTemplate.FourPlayersMediumWarpMap => new FourPlayersMediumWarpMapSettings(),
            MapTemplate.FourPlayersMediumMiniWarpMap => new FourPlayersMediumMiniWarpMapSettings(),
            MapTemplate.FourPlayersMediumDoubleWarpMap => new FourPlayersMediumDoubleWarpMapSettings(),
            MapTemplate.FivePlayersMediumMap => new FivePlayersMediumMapSettings(),
            MapTemplate.FivePlayersMediumHyperlineMap => new FivePlayersMediumHyperlineMapSettings(),
            MapTemplate.FivePlayersMediumDiamondMap => new FivePlayersMediumDiamondMapSettings(),
            MapTemplate.FivePlayersLargeFlatMap => new FivePlayersLargeFlatMapSettings(),
            MapTemplate.SixPlayersMediumMap => new SixPlayersMediumMapSettings(),
            MapTemplate.SixPlayersMediumSpiralMap => new SixPlayersMediumSpiralMapSettings(),
            MapTemplate.SixPlayersLargeMap => new SixPlayersLargeMapSettings(),
            MapTemplate.SevenPlayersLargeHyperlineMap => new SevenPlayersLargeHyperlineMapSettings(),
            MapTemplate.SevenPlayersLargeWarpMap => new SevenPlayersLargeWarpMapSettings(),
            MapTemplate.EightPlayersLargeMap => new EightPlayersLargeMapSettings(),
            MapTemplate.EightPlayersLargeWarpMap => new EightPlayersLargeWarpMapSettings(),
            _ => throw new ArgumentOutOfRangeException(nameof(mapTemplate), mapTemplate, null),
        };

        return Task.FromResult(mapSettings);
    }
}
