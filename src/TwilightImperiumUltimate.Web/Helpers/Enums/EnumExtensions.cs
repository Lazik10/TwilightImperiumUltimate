using System.Globalization;
using System.Resources;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.EightPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FivePlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FourPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SevenPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SixPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.TwoPlayers;

namespace TwilightImperiumUltimate.Web.Helpers.Enums;

public static class EnumExtensions
{
    private static readonly ResourceManager StringsResourceManager = new(Paths.ResourceNamespace_Strings, typeof(Program).Assembly);

    public static string GetDisplayName<TEnum>(this TEnum enumValue)
    {
        ArgumentNullException.ThrowIfNull(enumValue);

        string key = $"{enumValue.GetType().Name}_{enumValue}";
        string? displayName = StringsResourceManager.GetString(key, CultureInfo.InvariantCulture);
        return displayName ?? enumValue.GetType().ToString();
    }

    public static IReadOnlyCollection<KeyValuePair<TEnum, string>> GetEnumValuesWithDisplayNames<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
                   .Cast<TEnum>()
                   .Select(x => new KeyValuePair<TEnum, string>(x, x.GetDisplayName()))
                   .ToList();
    }

    public static IReadOnlyCollection<KeyValuePair<FactionName, string>> GetFactionValuesWithDisplayNames()
    {
        return Enum.GetValues(typeof(FactionName))
                   .Cast<FactionName>()
                   .Select(x => new KeyValuePair<FactionName, string>(x, x.GetFactionUIText(FactionResourceType.Title)))
                   .ToList();
    }

    public static string GetUIColor(this DraftColor color)
    {
        return color switch
        {
            DraftColor.None => "transparent",
            DraftColor.Red => "red",
            DraftColor.Blue => "deepskyblue",
            DraftColor.Green => "lawngreen",
            DraftColor.Yellow => "yellow",
            DraftColor.Purple => "rebeccapurple",
            DraftColor.Orange => "orange",
            DraftColor.Pink => "magenta",
            DraftColor.White => "white",
            _ => "transparent",
        };
    }

    public static string GetUIColor(this PlayerColor color)
    {
        return color switch
        {
            PlayerColor.Red => "red",
            PlayerColor.Blue => "blue",
            PlayerColor.Green => "green",
            PlayerColor.Yellow => "yellow",
            PlayerColor.Purple => "rebeccapurple",
            PlayerColor.Orange => "orange",
            PlayerColor.Pink => "magenta",
            PlayerColor.Black => "black",
            _ => "transparent",
        };
    }

    public static TextColor GetTextColor(this DraftColor color, bool transparent = false)
    {
        if (transparent)
        {
            return TextColor.Transparent;
        }

        return color switch
        {
            DraftColor.None => TextColor.White,
            DraftColor.Red => TextColor.Red,
            DraftColor.Blue => TextColor.Deepskyblue,
            DraftColor.Green => TextColor.Green,
            DraftColor.Yellow => TextColor.Yellow,
            DraftColor.Purple => TextColor.Purple,
            DraftColor.White => TextColor.White,
            DraftColor.Pink => TextColor.Pink,
            DraftColor.Orange => TextColor.Orange,
            _ => TextColor.White,
        };
    }

    public static MapTemplate GetCorrectMapTemplateFromFilter(this MapTemplateFilter mapFilter)
    {
        return mapFilter switch
        {
            MapTemplateFilter.CustomMap => MapTemplate.CustomMap,
            MapTemplateFilter.TwoPlayersMediumMap => MapTemplate.TwoPlayersMediumMap,
            MapTemplateFilter.ThreePlayersSmallMap => MapTemplate.ThreePlayersSmallMap,
            MapTemplateFilter.ThreePlayersSmallAlternateMap => MapTemplate.ThreePlayersSmallAlternateMap,
            MapTemplateFilter.ThreePlayersMediumTriangleMap => MapTemplate.ThreePlayersMediumTriangleMap,
            MapTemplateFilter.ThreePlayersMediumTriangleNarrowMap => MapTemplate.ThreePlayersMediumTriangleNarrowMap,
            MapTemplateFilter.ThreePlayersMediumSnowflakeMap => MapTemplate.ThreePlayersMediumSnowflakeMap,
            MapTemplateFilter.ThreePlayersMediumMantaRayMap => MapTemplate.ThreePlayersMediumMantaRayMap,
            MapTemplateFilter.ThreePlayersMediumTridentMap => MapTemplate.ThreePlayersMediumTridentMap,
            MapTemplateFilter.ThreePlayersMediumRexMap => MapTemplate.ThreePlayersMediumRexMap,
            MapTemplateFilter.FourPlayersMediumMap => MapTemplate.FourPlayersMediumMap,
            MapTemplateFilter.FourPlayersMediumHorizontalMap => MapTemplate.FourPlayersMediumHorizontalMap,
            MapTemplateFilter.FourPlayersMediumVerticalMap => MapTemplate.FourPlayersMediumVerticalMap,
            MapTemplateFilter.FourPlayersMediumGapsMap => MapTemplate.FourPlayersMediumGapsMap,
            MapTemplateFilter.FourPlayersMediumWarpMap => MapTemplate.FourPlayersMediumWarpMap,
            MapTemplateFilter.FourPlayersMediumMiniWarpMap => MapTemplate.FourPlayersMediumMiniWarpMap,
            MapTemplateFilter.FourPlayersMediumDoubleWarpMap => MapTemplate.FourPlayersMediumDoubleWarpMap,
            MapTemplateFilter.FivePlayersMediumMap => MapTemplate.FivePlayersMediumMap,
            MapTemplateFilter.FivePlayersMediumHyperlineMap => MapTemplate.FivePlayersMediumHyperlineMap,
            MapTemplateFilter.FivePlayersMediumDiamondMap => MapTemplate.FivePlayersMediumDiamondMap,
            MapTemplateFilter.FivePlayersLargeFlatMap => MapTemplate.FivePlayersLargeFlatMap,
            MapTemplateFilter.SixPlayersMediumMap => MapTemplate.SixPlayersMediumMap,
            MapTemplateFilter.SixPlayersMediumSpiralMap => MapTemplate.SixPlayersMediumSpiralMap,
            MapTemplateFilter.SixPlayersLargeMap => MapTemplate.SixPlayersLargeMap,
            MapTemplateFilter.SevenPlayersLargeHyperlineMap => MapTemplate.SevenPlayersLargeHyperlineMap,
            MapTemplateFilter.SevenPlayersLargeWarpMap => MapTemplate.SevenPlayersLargeWarpMap,
            MapTemplateFilter.EightPlayersLargeMap => MapTemplate.EightPlayersLargeMap,
            MapTemplateFilter.EightPlayersLargeWarpMap => MapTemplate.EightPlayersLargeWarpMap,
            MapTemplateFilter.All => MapTemplate.CustomMap,
            _ => MapTemplate.CustomMap,
        };
    }
}
