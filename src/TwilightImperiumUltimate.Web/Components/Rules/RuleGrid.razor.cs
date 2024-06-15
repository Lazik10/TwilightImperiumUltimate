using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Resources;
using TwilightImperiumUltimate.Web.Helpers.Text;
using TwilightImperiumUltimate.Web.Models.Rules;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleGrid
{
    private const string _searchColor = "yellow";
    private const int _minimumSearchLength = 3;
    private List<Rule> _rules = [];

    private List<TransformedRule> FilteredAndTransformedRules { get; set; } = [];

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var result = await HttpClient.GetAsync<List<Rule>>(Paths.ApiPath_Rules);
        _rules = [.. result.Where(x => x.RuleCategory != RuleCategory.None).OrderBy(x => x.RuleCategory)];
        FilteredAndTransformedRules = GetTransformedRules();
    }

    private void GetFilteredRules(string search)
    {
        if (!string.IsNullOrEmpty(search) && search.Length < _minimumSearchLength)
        {
            FilteredAndTransformedRules = GetTransformedRules();
        }
        else
        {
            var transformedRules = GetRulesWithHighlightedSearchWord(search);

            FilteredAndTransformedRules = transformedRules
                .Where(x => x.RuleTitle.Contains(search, StringComparison.CurrentCultureIgnoreCase) || x.Content.Contains(search, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
    }

    private List<TransformedRule> GetTransformedRules()
    {
        return _rules.Select(x => new TransformedRule(x.RuleCategory, x.RuleCategory.GetRuleTitleUIText(), x.RuleCategory.GetRuleUIText(), x.Language)).ToList();
    }

    private List<TransformedRule> GetRulesWithHighlightedSearchWord(string search)
    {
        return _rules.Select(x => new TransformedRule(
            x.RuleCategory,
            x.RuleCategory.GetRuleTitleUIText().HighlightSearchWord(search, _searchColor, false),
            x.RuleCategory.GetRuleUIText().HighlightSearchWord(search, _searchColor, true),
            x.Language)).ToList();
    }
}
