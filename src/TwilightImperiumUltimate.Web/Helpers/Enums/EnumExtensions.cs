using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using TwilightImperiumUltimate.Web.Pages.Community;

namespace TwilightImperiumUltimate.Web.Helpers.Enums;

public static class EnumExtensions
{
    private static readonly ResourceManager StringsResourceManager = new(Paths.ResourceNamespace_Strings, typeof(Program).Assembly);
    private static readonly ResourceManager CardNameResourceManager = new(Paths.ResourceNamespace_CardNames, typeof(Program).Assembly);
    private static readonly ResourceManager TiglFactionsResourceManager = new ResourceManager(Paths.ResourceNamespace_TiglFactions, typeof(Program).Assembly);
    private static readonly ResourceManager LeadersResourceManager = new ResourceManager(Paths.ResourceNamespace_Leaders, typeof(Program).Assembly);

    public static string GetDisplayName<TEnum>(this TEnum enumValue)
    {
        ArgumentNullException.ThrowIfNull(enumValue);

        string key = $"{enumValue.GetType().Name}_{enumValue}";
        string? displayName = StringsResourceManager.GetString(key, CultureInfo.InvariantCulture);
        return displayName ?? enumValue.GetType().ToString();
    }

    public static string GetCardDisplayName<TEnum>(this TEnum enumValue)
    {
        ArgumentNullException.ThrowIfNull(enumValue);

        string key = $"{enumValue.GetType().Name}_{enumValue}";
        string? displayName = CardNameResourceManager.GetString(key, CultureInfo.InvariantCulture);
        return displayName ?? string.Empty;
    }

    public static string GetDisplayName(this TiglPrestigeRank enumValue)
    {
        string key = enumValue.ToString();
        string? displayName = LeadersResourceManager.GetString(key, CultureInfo.InvariantCulture);
        return displayName ?? string.Empty;
    }

    public static string GetDisplayName(this TiglFactionName enumValue)
    {
        string key = enumValue.ToString();
        string? displayName = TiglFactionsResourceManager.GetString(key, CultureInfo.InvariantCulture);
        return displayName ?? string.Empty;
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

    public static TextColor GetTextColor(this PlayerColor color, bool substituteBlack = false)
    {
        if (color == PlayerColor.Black && substituteBlack)
            return TextColor.Grey;

        return color switch
        {
            PlayerColor.Red => TextColor.Red,
            PlayerColor.Blue => TextColor.Deepskyblue,
            PlayerColor.Green => TextColor.Green,
            PlayerColor.Yellow => TextColor.Yellow,
            PlayerColor.Purple => TextColor.Purple,
            PlayerColor.Pink => TextColor.Pink,
            PlayerColor.Orange => TextColor.Orange,
            PlayerColor.Black => TextColor.Black,
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
            MapTemplateFilter.ThreePlayersMediumHyperlineMap => MapTemplate.ThreePlayersMediumHyperlineMap,
            MapTemplateFilter.ThreePlayersMediumTriangleMap => MapTemplate.ThreePlayersMediumTriangleMap,
            MapTemplateFilter.ThreePlayersMediumTriangleNarrowMap => MapTemplate.ThreePlayersMediumTriangleNarrowMap,
            MapTemplateFilter.ThreePlayersMediumSnowflakeMap => MapTemplate.ThreePlayersMediumSnowflakeMap,
            MapTemplateFilter.ThreePlayersMediumMantaRayMap => MapTemplate.ThreePlayersMediumMantaRayMap,
            MapTemplateFilter.ThreePlayersMediumTridentMap => MapTemplate.ThreePlayersMediumTridentMap,
            MapTemplateFilter.ThreePlayersMediumRexMap => MapTemplate.ThreePlayersMediumRexMap,
            MapTemplateFilter.FourPlayersMediumHyperlineMap => MapTemplate.FourPlayersMediumHyperlineMap,
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

    public static string ConvertToString(this TextColor color)
    {
        return color switch
        {
            TextColor.White => "white",
            TextColor.Red => "red",
            TextColor.Green => "lawngreen",
            TextColor.Blue => "blue",
            TextColor.Yellow => "yellow",
            TextColor.Deepskyblue => "deepskyblue",
            TextColor.Purple => "purple",
            TextColor.Pink => "magenta",
            TextColor.Orange => "orange",
            TextColor.Grey => "grey",
            TextColor.DarkGreen => "darkgreen",
            TextColor.Black => "black",

            TextColor.Unranked => "white",
            TextColor.Minister => "#3498db",
            TextColor.Agent => "#206694",
            TextColor.Commander => "#b2a8e4",
            TextColor.Hero => "#816de6",
            TextColor.GalacticThreat => "#e74c3c",
            TextColor.Pmbg => "#f1c40f",

            _ => "white",
        };
    }

    public static string GetAlignString(this AlignItems alignItems)
    {
        return alignItems switch
        {
            AlignItems.Baseline => "baseline;",
            AlignItems.Stretch => "stretch;",
            AlignItems.Center => "center;",
            AlignItems.FlexEnd => "flex-end;",
            AlignItems.FlexStart => "flex-start;",
            _ => "center;",
        };
    }

    public static string GetJustifyString(this JustifyContent justifyContent)
    {
        return justifyContent switch
        {
            JustifyContent.Center => "center;",
            JustifyContent.FlexEnd => "flex-end;",
            JustifyContent.SpaceAround => "space-around;",
            JustifyContent.SpaceBetween => "space-between;",
            JustifyContent.FlexStart => "flex-start;",
            _ => "center;",
        };
    }

    public static TextColor GetRankColor(this TiglRankName rank)
    {
        return rank switch
        {
            TiglRankName.Unranked or TiglRankName.Thrall => TextColor.Unranked,
            TiglRankName.Minister or TiglRankName.Acolyte => TextColor.Minister,
            TiglRankName.Agent or TiglRankName.Legionnaire => TextColor.Agent,
            TiglRankName.Commander or TiglRankName.Starlancer => TextColor.Commander,
            TiglRankName.Hero or TiglRankName.GeneSorcerer or TiglRankName.IxthLord or TiglRankName.Archon => TextColor.Hero,
            _ => TextColor.White,
        };
    }
}
