using System.Globalization;

namespace TwilightImperiumUltimate.Web.Helpers.Format;

public static class NumberFormat
{
    public static string ToSignedFormat(this double number, int decimals)
    {
        if (decimals <= 0)
            return number.ToString("+0;-0;0", CultureInfo.InvariantCulture);

        var zeros = new string('0', decimals);
        return number.ToString($"+0.{zeros};-0.{zeros};0.{zeros}", CultureInfo.InvariantCulture);
    }
}
