using System.Globalization;
using TwilightImperiumUltimate.Web.Pages.Community;

namespace TwilightImperiumUltimate.Web.Helpers.Numbers;

public static class NumberFormatExtensions
{
    public static string ToStringWithPrecision(this float number, int decimalValues)
    {
        return number.ToString($"N{decimalValues}", CultureInfo.InvariantCulture);
    }

    public static string ToStringWithPrecisionAndPercentage(this float number, int decimalValues)
    {
        return $"{number.ToStringWithPrecision(decimalValues)}%";
    }

    public static TextColor GetWinrateColor(this float winrate)
    {
        if (winrate > 16.67f) return TextColor.Green;
        if (winrate > 12.0f) return TextColor.Yellow;
        if (winrate > 8.0f) return TextColor.Orange;
        return TextColor.Red;
    }

    public static TextColor GetAverageVpColor(this float winrate)
    {
        if (winrate >= 8.0f) return TextColor.Green;
        if (winrate >= 6.0f) return TextColor.Yellow;
        if (winrate >= 4.0f) return TextColor.Orange;
        return TextColor.Red;
    }

    public static TextColor GetAverageVpPercentageColor(this float winrate)
    {
        if (winrate >= 80.0f) return TextColor.Green;
        if (winrate >= 60.0f) return TextColor.Yellow;
        if (winrate >= 40.0f) return TextColor.Orange;
        return TextColor.Red;
    }

    public static TextColor GetWinrateColor(this double winrate)
    {
        if (winrate > 16.67) return TextColor.Green;
        if (winrate > 12.0) return TextColor.Yellow;
        if (winrate > 8.0) return TextColor.Orange;
        return TextColor.Red;
    }

    public static TextColor GetAverageVpColor(this double winrate)
    {
        if (winrate >= 8.0) return TextColor.Green;
        if (winrate >= 6.0) return TextColor.Yellow;
        if (winrate >= 4.0) return TextColor.Orange;
        return TextColor.Red;
    }

    public static TextColor GetAverageVpPercentageColor(this double winrate)
    {
        if (winrate >= 80.0) return TextColor.Green;
        if (winrate >= 60.0) return TextColor.Yellow;
        if (winrate >= 40.0) return TextColor.Orange;
        return TextColor.Red;
    }
}
