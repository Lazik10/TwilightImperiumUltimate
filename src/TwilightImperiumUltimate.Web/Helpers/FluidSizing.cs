using System.Globalization;

namespace TwilightImperiumUltimate.Web.Helpers;

/// <summary>
/// Converts fixed pixel-based sizes into fluid CSS <c>clamp()</c> expressions, so existing pixel-based
/// component parameters (e.g. <c>Label.FontSize</c>, <c>VerticalSpace.Height</c>, <c>HorizontalSpace.Width</c>)
/// become responsive on small screens without requiring any changes to their public APIs or call sites.
/// </summary>
public static class FluidSizing
{
    private const double PixelsPerRem = 16.0;

    /// <summary>
    /// Builds a <c>clamp(min, min + Nvw, max)</c> value for a font-size, shrinking down to a floor of
    /// 75% of the original size (never below 10px / 0.625rem) on narrow screens.
    /// </summary>
    public static string GetFluidFontSize(int fontSizePx) => BuildClamp(fontSizePx, minRatio: 0.75, minFloorRem: 0.625, vwCoefficient: 0.5);

    /// <summary>
    /// Builds a <c>clamp(min, min + Nvw, max)</c> value for a spacing dimension (height/width), shrinking
    /// down to 50% of the original size on narrow screens, since whitespace can safely shrink more
    /// aggressively than text.
    /// </summary>
    public static string GetFluidSpacing(int sizePx) => BuildClamp(sizePx, minRatio: 0.5, minFloorRem: 0.125, vwCoefficient: 1.0);

    private static string BuildClamp(int sizePx, double minRatio, double minFloorRem, double vwCoefficient)
    {
        var maxRem = sizePx / PixelsPerRem;
        var minRem = Math.Min(Math.Max(maxRem * minRatio, minFloorRem), maxRem);

        var min = minRem.ToString("0.###", CultureInfo.InvariantCulture);
        var max = maxRem.ToString("0.###", CultureInfo.InvariantCulture);
        var vw = vwCoefficient.ToString("0.###", CultureInfo.InvariantCulture);
        return $"clamp({min}rem, {min}rem + {vw}vw, {max}rem)";
    }
}
