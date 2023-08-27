using System.Globalization;
using System.Resources;

namespace TwilightImperiumUltimate.Web.Helpers.Resources;

public static class ResourceProvider
{
    private static readonly ResourceManager UIResourceManager = new("ui", typeof(Program).Assembly);

    public static string ToUIText(this Enum key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return UIResourceManager.GetString(key.ToString(), CultureInfo.InvariantCulture) ?? throw new InvalidOperationException($"Resource with key '{key}' not found.");
    }
}
