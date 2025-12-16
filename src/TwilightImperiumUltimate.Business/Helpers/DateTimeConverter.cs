namespace TwilightImperiumUltimate.Business.Helpers;

internal static class DateTimeConverter
{
    public static DateOnly ToDateOnly(this long unixTimeMilliseconds)
    {
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds).UtcDateTime;
        return DateOnly.FromDateTime(dateTime);
    }
}
