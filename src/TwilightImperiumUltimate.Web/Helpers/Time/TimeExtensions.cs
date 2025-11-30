using System.Globalization;

namespace TwilightImperiumUltimate.Web.Helpers.Time;

public static class TimeExtensions
{
    public static string FormaToDays(this TimeSpan duration)
    {
        List<string> parts = new List<string>();

        if (duration.Days > 0)
            parts.Add($"{duration.Days:D2} d");
        if (duration.Hours > 0)
            parts.Add($"{duration.Hours:D2} h");
        if (duration.Minutes > 0)
            parts.Add($"{duration.Minutes:D2} m");

        return parts.Count > 0 ? string.Join(" ", parts) : "0s";
    }

    public static string FormatToDaysDuration(this long? duration)
    {
        if (duration is null)
            return "Unknown";

        if (duration.Value == 0)
            return "0s";

        return TimeSpan.FromMilliseconds(duration.Value).FormaToDays();
    }

    public static string FormatToDaysDuration(this long duration)
    {
        if (duration == 0)
            return "0s";

        return TimeSpan.FromMilliseconds(duration).FormaToDays();
    }

    public static string FormatToDateString(this long? timestamp)
    {
        if (timestamp is null)
            return "Unknown";

        var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value).ToLocalTime().DateTime;
        return dateTime.ToString("dd/MM/yyyy - HH:mm", CultureInfo.InvariantCulture);
    }

    public static string FormatToDateString(this long timestamp)
    {
        if (timestamp == 0)
            return "Unknown";

        var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToLocalTime().DateTime;
        return dateTime.ToString("dd/MM/yyyy - HH:mm", CultureInfo.InvariantCulture);
    }
}
