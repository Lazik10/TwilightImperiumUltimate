namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleGrid
{
    private const string _highlightColor = "yellow";
    private const int _minimumSearchLength = 3;
    private bool _showNotes = true;
    private IReadOnlyCollection<RuleModel> _rules = new List<RuleModel>();

    [Parameter]
    public string SearchWord { get; set; } = string.Empty;

    [Parameter]
    public int? RuleId { get; set; }

    private IReadOnlyCollection<TransformedRule> FilteredAndTransformedRules { get; set; } = [];

    private IReadOnlyCollection<RuleSection> RuleSections { get; set; } = [];

    private TransformedRule? SelectedRule { get; set; }

    private TransformedRule? PreviousRule { get; set; }

    private TransformedRule? NextRule { get; set; }

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await InitializeRules();

        if (!string.IsNullOrEmpty(SearchWord))
        {
            GetFilteredRulesAndNotes(SearchWord);
        }

        SetSelectedRule();
    }

    protected override void OnParametersSet()
    {
        if (_rules.Count > 0)
        {
            SetSelectedRule();
        }
    }

    private void GetFilteredRulesAndNotes(string search)
    {
        if (string.IsNullOrWhiteSpace(search) || search.Length < _minimumSearchLength)
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

        RuleSections = GetRuleSections();
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
            RuleSections = GetRuleSections();
        }
    }

    private void ToggleNotes()
    {
        _showNotes = !_showNotes;
        StateHasChanged();
    }

    private List<RuleSection> GetRuleSections()
    {
        return FilteredAndTransformedRules
            .GroupBy(rule => GetSectionTitle(rule.RuleTitle))
            .Select(group => new RuleSection(group.Key, group.ToList()))
            .ToList();
    }

    private void SetSelectedRule()
    {
        var rules = GetTransformedRules();
        if (RuleId is null)
        {
            SelectedRule = null;
            PreviousRule = null;
            NextRule = null;
            return;
        }

        SelectedRule = rules.FirstOrDefault(rule => GetRuleId(rule) == RuleId);
        if (SelectedRule is null)
        {
            PreviousRule = null;
            NextRule = null;
            return;
        }

        var selectedIndex = rules.FindIndex(rule => rule.RuleCategory == SelectedRule.RuleCategory);
        PreviousRule = selectedIndex > 0 ? rules[selectedIndex - 1] : null;
        NextRule = selectedIndex >= 0 && selectedIndex < rules.Count - 1 ? rules[selectedIndex + 1] : null;
    }

    private static string GetSectionTitle(string title)
    {
        var plainTitle = StripMarkup(title);
        var firstLetter = plainTitle.FirstOrDefault(char.IsLetterOrDigit);
        return firstLetter == default ? "#" : char.ToUpperInvariant(firstLetter).ToString();
    }

    private static int GetRuleId(TransformedRule rule) => (int)rule.RuleCategory;

    private static string GetRulePath(TransformedRule rule) => $"/rules/{GetRuleId(rule)}";

    private string GetNotesToggleClass() => _showNotes
        ? "rules-notes-toggle rules-notes-toggle-on handel"
        : "rules-notes-toggle handel";

    private string GetNotesToggleText() => _showNotes ? "On" : "Off";

    private void NavigateToSearch(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            NavigationManager.NavigateTo("/rules");
            return;
        }

        NavigationManager.NavigateTo($"/rules?search={Uri.EscapeDataString(search)}");
    }

    private static string StripMarkup(string text)
    {
        return System.Text.RegularExpressions.Regex.Replace(text, "<.*?>", string.Empty);
    }

    private static string GetPlainTitle(string title) => StripMarkup(title);

    private sealed record RuleSection(string Title, IReadOnlyCollection<TransformedRule> Rules);
}
