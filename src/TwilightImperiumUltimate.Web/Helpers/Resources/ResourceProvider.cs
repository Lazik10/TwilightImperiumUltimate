using Serilog;
using System.Globalization;
using System.Resources;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Helpers.Resources;

public static class ResourceProvider
{
    private static readonly ResourceManager FactionResourceManager = new(Paths.ResourceNamespace_FactionInfo, typeof(Program).Assembly);
    private static readonly ResourceManager RuleResourceManager = new(Paths.ResourceNamespace_Rules, typeof(Program).Assembly);

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

    public static string GetRuleUIText(this RuleCategory ruleCategory)
    {
        ArgumentNullException.ThrowIfNull(ruleCategory);

        var lookupString = $"RuleCategory_{ruleCategory}";
        var result = RuleResourceManager.GetString(lookupString, CultureInfo.CurrentCulture);

        if (result is not null)
        {
            return result;
        }
        else
        {
            Log.Error($"Could not find resource for {lookupString} in {nameof(RuleResourceManager)}");
            return string.Empty;
        }
    }

    public static string GetRuleTitleUIText(this RuleCategory ruleCategory)
    {
        ArgumentNullException.ThrowIfNull(ruleCategory);

        var lookupString = $"RuleCategoryTitle_{ruleCategory}";
        var result = RuleResourceManager.GetString(lookupString, CultureInfo.CurrentCulture);

        if (result is not null)
        {
            return result;
        }
        else
        {
            Log.Error($"Could not find resource for {lookupString} in {nameof(RuleResourceManager)}");
            return string.Empty;
        }
    }
}
