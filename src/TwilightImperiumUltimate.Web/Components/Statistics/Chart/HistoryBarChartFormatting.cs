using System.Globalization;
using TwilightImperiumUltimate.Web.Components.Async.Statistics;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

internal static class HistoryBarChartFormatting
{
    internal static string FormatCategory(object value)
    {
        var key = value?.ToString() ?? string.Empty;
        if (key.StartsWith("Y-", StringComparison.Ordinal))
            return key[2..];

        if (key.Length == 6 && int.TryParse(key[4..6], NumberStyles.Integer, CultureInfo.InvariantCulture, out var month))
            return CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(month);

        return key;
    }

    internal static string FormatValue(double value) =>
        value <= 0 ? string.Empty : value.ToString(CultureInfo.InvariantCulture);

    internal static int GetChartHeight(int pointCount) => Math.Max(520, 180 + (pointCount * 24));
}
