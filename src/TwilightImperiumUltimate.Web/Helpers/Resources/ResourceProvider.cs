using Serilog;
using System.Globalization;
using System.Resources;

namespace TwilightImperiumUltimate.Web.Helpers.Resources;

public static class ResourceProvider
{
    private static readonly ResourceManager FactionResourceManager = new(Paths.ResourceNamespace_FactionInfo, typeof(Program).Assembly);
    private static readonly ResourceManager RuleResourceManager = new(Paths.ResourceNamespace_Rules, typeof(Program).Assembly);
    private static readonly ResourceManager RuleNotesResourceManager = new(Paths.ResourceNamespace_RuleNotes, typeof(Program).Assembly);

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
            Log.Error("Could not find resource for {LookupString} in {FactionResourceManager}", lookupString, nameof(FactionResourceManager));
            return string.Empty;
        }
    }

    public static string GetFactionUIText(this AsyncFactionName factionName, FactionResourceType resourceType)
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
            Log.Error("Could not find resource for {LookupString} in {FactionResourceManager}", lookupString, nameof(FactionResourceManager));
            return string.Empty;
        }
    }

    public static string GetComponentNotesText(this string componentName)
    {
        ArgumentNullException.ThrowIfNull(componentName);

        var lookupString = $"{componentName}";
        var result = RuleNotesResourceManager.GetString(lookupString, CultureInfo.CurrentCulture);

        if (result is not null)
        {
            return result;
        }
        else
        {
            Log.Error("Could not find resource for {LookupString} in {RuleNotesResourceManager}", lookupString, nameof(RuleNotesResourceManager));
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
            Log.Error("Could not find resource for {LookupString} in {RuleResourceManager}", lookupString, nameof(RuleResourceManager));
            return string.Empty;
        }
    }

    public static string GetRuleUINoteText(this RuleCategory ruleCategory)
    {
        ArgumentNullException.ThrowIfNull(ruleCategory);

        var lookupString = $"RuleCategory_{ruleCategory}_Notes";
        var result = RuleResourceManager.GetString(lookupString, CultureInfo.CurrentCulture);

        if (result is not null)
        {
            return result;
        }
        else
        {
            Log.Error("Could not find resource for {LookupString} in {RuleResourceManager}", lookupString, nameof(RuleResourceManager));
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
            Log.Error("Could not find resource for {LookupString} in {RuleResourceManager}", lookupString, nameof(RuleResourceManager));
            return string.Empty;
        }
    }
}
