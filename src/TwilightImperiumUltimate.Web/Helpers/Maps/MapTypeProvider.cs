using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.Custom;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.EightPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FivePlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FourPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SevenPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SixPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.TwoPlayers;
using TwilightImperiumUltimate.Web.Components.PreviewMaps;

namespace TwilightImperiumUltimate.Web.Helpers.Maps;

public static class MapTypeProvider
{
    public static Type GetMapTypeFromMapTemplate(this MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.CustomMap => typeof(CustomMap),
            MapTemplate.TwoPlayersMediumMap => typeof(TwoPlayersMediumMap),
            MapTemplate.ThreePlayersSmallMap => typeof(ThreePlayersSmallMap),
            MapTemplate.ThreePlayersSmallAlternateMap => typeof(ThreePlayersSmallAlternateMap),
            MapTemplate.ThreePlayersMediumTriangleMap => typeof(ThreePlayersMediumTriangleMap),
            MapTemplate.ThreePlayersMediumTriangleNarrowMap => typeof(ThreePlayersMediumTriangleNarrowMap),
            MapTemplate.ThreePlayersMediumSnowflakeMap => typeof(ThreePlayersMediumSnowflakeMap),
            MapTemplate.ThreePlayersMediumMantaRayMap => typeof(ThreePlayersMediumMantaRayMap),
            MapTemplate.ThreePlayersMediumTridentMap => typeof(ThreePlayersMediumTridentMap),
            MapTemplate.ThreePlayersMediumRexMap => typeof(ThreePlayersMediumRexMap),
            MapTemplate.FourPlayersMediumMap => typeof(FourPlayersMediumMap),
            MapTemplate.FourPlayersMediumHorizontalMap => typeof(FourPlayersMediumHorizontalMap),
            MapTemplate.FourPlayersMediumVerticalMap => typeof(FourPlayersMediumVerticalMap),
            MapTemplate.FourPlayersMediumGapsMap => typeof(FourPlayersMediumGapsMap),
            MapTemplate.FourPlayersMediumWarpMap => typeof(FourPlayersMediumWarpMap),
            MapTemplate.FourPlayersMediumMiniWarpMap => typeof(FourPlayersMediumMiniWarpMap),
            MapTemplate.FourPlayersMediumDoubleWarpMap => typeof(FourPlayersMediumDoubleWarpMap),
            MapTemplate.FivePlayersMediumMap => typeof(FivePlayersMediumMap),
            MapTemplate.FivePlayersMediumHyperlineMap => typeof(FivePlayersMediumHyperlineMap),
            MapTemplate.FivePlayersMediumDiamondMap => typeof(FivePlayersMediumDiamondMap),
            MapTemplate.FivePlayersLargeFlatMap => typeof(FivePlayersLargeFlatMap),
            MapTemplate.SixPlayersMediumMap => typeof(SixPlayersMediumMap),
            MapTemplate.SixPlayersMediumSpiralMap => typeof(SixPlayersMediumSpiralMap),
            MapTemplate.SixPlayersLargeMap => typeof(SixPlayersLargeMap),
            MapTemplate.SevenPlayersLargeHyperlineMap => typeof(SevenPlayersLargeHyperlineMap),
            MapTemplate.SevenPlayersLargeWarpMap => typeof(SevenPlayersLargeWarpMap),
            MapTemplate.EightPlayersLargeMap => typeof(EightPlayersLargeMap),
            MapTemplate.EightPlayersLargeWarpMap => typeof(EightPlayersLargeWarpMap),
            _ => throw new NotImplementedException(),
        };
    }

    public static Type GetPreviewMapTypeFromMapTemplate(this MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.CustomMap => typeof(CustomMapPreview),
            MapTemplate.TwoPlayersMediumMap => typeof(TwoPlayersMediumMapPreview),
            MapTemplate.ThreePlayersSmallMap => typeof(ThreePlayersSmallMapPreview),
            MapTemplate.ThreePlayersSmallAlternateMap => typeof(ThreePlayersSmallAlternateMapPreview),
            MapTemplate.ThreePlayersMediumTriangleMap => typeof(ThreePlayersMediumTriangleMapPreview),
            MapTemplate.ThreePlayersMediumTriangleNarrowMap => typeof(ThreePlayersMediumTriangleNarrowMapPreview),
            MapTemplate.ThreePlayersMediumSnowflakeMap => typeof(ThreePlayersMediumSnowflakeMapPreview),
            MapTemplate.ThreePlayersMediumMantaRayMap => typeof(ThreePlayersMediumMantaRayMapPreview),
            MapTemplate.ThreePlayersMediumTridentMap => typeof(ThreePlayersMediumTridentMapPreview),
            MapTemplate.ThreePlayersMediumRexMap => typeof(ThreePlayersMediumRexMapPreview),
            MapTemplate.FourPlayersMediumMap => typeof(FourPlayersMediumMapPreview),
            MapTemplate.FourPlayersMediumHorizontalMap => typeof(FourPlayersMediumHorizontalMapPreview),
            MapTemplate.FourPlayersMediumVerticalMap => typeof(FourPlayersMediumVerticalMapPreview),
            MapTemplate.FourPlayersMediumGapsMap => typeof(FourPlayersMediumGapsMapPreview),
            MapTemplate.FourPlayersMediumWarpMap => typeof(FourPlayersMediumWarpMapPreview),
            MapTemplate.FourPlayersMediumMiniWarpMap => typeof(FourPlayersMediumMiniWarpMapPreview),
            MapTemplate.FourPlayersMediumDoubleWarpMap => typeof(FourPlayersMediumDoubleWarpMapPreview),
            MapTemplate.FivePlayersMediumMap => typeof(FivePlayersMediumMapPreview),
            MapTemplate.FivePlayersMediumHyperlineMap => typeof(FivePlayersMediumHyperlineMapPreview),
            MapTemplate.FivePlayersMediumDiamondMap => typeof(FivePlayersMediumDiamondMapPreview),
            MapTemplate.FivePlayersLargeFlatMap => typeof(FivePlayersLargeFlatMapPreview),
            MapTemplate.SixPlayersMediumMap => typeof(SixPlayersMediumMapPreview),
            MapTemplate.SixPlayersMediumSpiralMap => typeof(SixPlayersMediumSpiralMapPreview),
            MapTemplate.SixPlayersLargeMap => typeof(SixPlayersLargeMap),
            MapTemplate.SevenPlayersLargeHyperlineMap => typeof(SevenPlayersLargeHyperlineMapPreview),
            MapTemplate.SevenPlayersLargeWarpMap => typeof(SevenPlayersLargeWarpMapPreview),
            MapTemplate.EightPlayersLargeMap => typeof(EightPlayersLargeMapPreview),
            MapTemplate.EightPlayersLargeWarpMap => typeof(EightPlayersLargeWarpMapPreview),
            _ => typeof(SixPlayersMediumMapPreview),
        };
    }

    public static bool IsSupportedPreviewForMiltyDraftMap(this MapTemplate mapTemplate)
    {
        var supportedMapTemplates = new List<MapTemplate>()
        {
            MapTemplate.FivePlayersMediumHyperlineMap,
            MapTemplate.SixPlayersMediumMap,
            MapTemplate.SevenPlayersLargeWarpMap,
            MapTemplate.EightPlayersLargeWarpMap,
        };

        return supportedMapTemplates.Contains(mapTemplate);
    }
}
