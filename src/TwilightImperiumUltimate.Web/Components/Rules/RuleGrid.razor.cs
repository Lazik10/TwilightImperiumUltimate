namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleGrid
{
    private const string _highlightColor = "yellow";
    private const int _minimumSearchLength = 3;
    private bool _showNotes = true;
    private IReadOnlyCollection<RuleModel> _rules = new List<RuleModel>();

    private IReadOnlyCollection<TransformedRule> FilteredAndTransformedRules { get; set; } = [];

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await InitializeRules();
    }

    private void GetFilteredRulesAndNotes(string search)
    {
        if (!string.IsNullOrEmpty(search) && search.Length < _minimumSearchLength)
        {
            FilteredAndTransformedRules = GetTransformedRules();
        }
        else
        {
            var transformedRules = GetRulesWithHighlightedSearchWord(search);

            FilteredAndTransformedRules = transformedRules
                .Where(x =>
                    x.RuleTitle.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                    || x.Content.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                    || x.NotesContent.Contains(search, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
    }

    private List<TransformedRule> GetTransformedRules()
    {
        return _rules.Select(x =>
            new TransformedRule(
                x.RuleCategory,
                x.RuleCategory.GetRuleTitleUIText(),
                x.RuleCategory.GetRuleUIText(),
                x.RuleCategory.GetRuleUINoteText()))
            .ToList();
    }

    private List<TransformedRule> GetRulesWithHighlightedSearchWord(string search)
    {
        return _rules.Select(x => new TransformedRule(
            x.RuleCategory,
            x.RuleCategory.GetRuleTitleUIText().HighlightSearchWord(search, _highlightColor, false),
            x.RuleCategory.GetRuleUIText().HighlightSearchWord(search, _highlightColor, true),
            x.RuleCategory.GetRuleUINoteText().HighlightSearchWord(search, _highlightColor, true)))
            .ToList();
    }

    private async Task InitializeRules()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RuleDto>>>(Paths.ApiPath_Rules, default);
        if (statusCode == HttpStatusCode.OK)
        {
            _rules = Mapper.Map<List<RuleModel>>(response!.Data!.Items);
            FilteredAndTransformedRules = GetTransformedRules();
        }
    }

    private void ToggleNotes()
    {
        _showNotes = !_showNotes;
        StateHasChanged();
    }
}
