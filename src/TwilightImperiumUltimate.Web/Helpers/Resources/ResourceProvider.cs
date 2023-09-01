using Serilog;
using System.Globalization;
using System.Resources;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Helpers.Resources;

public static class ResourceProvider
{
    private static readonly ResourceManager FactionResourceManager = new(Paths.ResourceNamespace_FactionInfo, typeof(Program).Assembly);

    public static string GetFactionUIText(this FactionName factionName, FactionResourceType resourceType)
    {
        ArgumentNullException.ThrowIfNull(factionName);
        ArgumentNullException.ThrowIfNull(resourceType);

        var lookupString = $"{factionName}_{resourceType}";
        var result = FactionResourceManager.GetString(lookupString, CultureInfo.CurrentCulture);

        if (result is not null)
        {
            return result;
        }
        else
        {
            Log.Error($"Could not find resource for {lookupString} in {nameof(FactionResourceManager)}");
            return string.Empty;
        }
    }
}
