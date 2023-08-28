using System.Globalization;
using System.Resources;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Helpers.Resources;

public static class ResourceProvider
{
    private static readonly ResourceManager FactionResourceManager = new("TwilightImperiumUltimate.Web.Resources.FactionsInfo", typeof(Program).Assembly);

    public static string GetFactionUIResource(this FactionName factionName, FactionResourceType resourceType)
    {
        ArgumentNullException.ThrowIfNull(factionName);
        ArgumentNullException.ThrowIfNull(resourceType);

        var lookupString = $"{factionName}_{resourceType}";
        return FactionResourceManager.GetString(lookupString, CultureInfo.InvariantCulture) ?? throw new InvalidOperationException($"Resource with key '{factionName}_{resourceType}' not found.");
    }
}
