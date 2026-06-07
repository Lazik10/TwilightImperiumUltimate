using System.Collections.Immutable;
using System.Globalization;
using System.Net;
using TwilightImperiumUltimate.Web.Services.Rules;

namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleGrid
{
    private const string HighlightColor = "yellow";

    private ImmutableArray<RuleDocument> _documents = [];
    private string _searchTerm = string.Empty;
    private bool _showNotes = true;

    [Parameter]
    public string SearchWord { get; set; } = string.Empty;

    [Parameter]
    public string? Letter { get; set; }

    [Parameter]
    public int? RuleId { get; set; }

    private IReadOnlyCollection<RuleSection> RuleSections { get; set; } = [];

    private IReadOnlyCollection<RulesIndexItem> LetterFilterItems { get; set; } = [];

    private int FilteredRuleCount { get; set; }

    private string ActiveLetter { get; set; } = string.Empty;

    private TransformedRule? SelectedRule { get; set; }

    private TransformedRule? PreviousRule { get; set; }

    private TransformedRule? NextRule { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IRulesCatalogCache<ImmutableArray<RuleDocument>> CatalogCache { get; set; } = default!;

    protected override void OnInitialized()
    {
        _documents = CatalogCache.GetOrCreate(
            CultureInfo.CurrentUICulture.Name,
            dependencyVersion: 0,
            BuildRuleIndex);
    }

    protected override void OnParametersSet()
    {
        _searchTerm = SearchWord ?? string.Empty;
        ApplyFilters(Letter);
        SetSelectedRule();
    }

    private void ApplyFilters(string? requestedLetter)
    {
        var searchMatches = string.IsNullOrWhiteSpace(_searchTerm)
            ? _documents
            : _documents
                .Where(document => document.SearchText.Contains(
                    _searchTerm,
                    StringComparison.CurrentCultureIgnoreCase))
                .ToImmutableArray();

        var availableLetters = searchMatches.Select(document => document.Letter).ToHashSet();
        LetterFilterItems = RulesAlphabet.Build(availableLetters);
        ActiveLetter = RulesAlphabet.Normalize(requestedLetter, availableLetters);

        var filteredDocuments = string.IsNullOrEmpty(ActiveLetter)
            ? searchMatches
            : searchMatches
                .Where(document => document.Letter == ActiveLetter)
                .ToImmutableArray();

        var transformedRules = filteredDocuments
            .Select(document => new TransformedRuleDocument(
                document.Letter,
                ToDisplayRule(document)))
            .ToImmutableArray();

        FilteredRuleCount = transformedRules.Length;

        RuleSections = transformedRules
            .GroupBy(document => document.Letter)
            .Select(group => new RuleSection(
                group.Key,
                group.Select(document => document.Rule).ToImmutableArray()))
            .ToImmutableArray();
    }

    private void SetSelectedRule()
    {
        if (RuleId is null)
        {
            SelectedRule = null;
            PreviousRule = null;
            NextRule = null;
            return;
        }

        var selectedDocument = _documents.FirstOrDefault(document => document.Id == RuleId);
        if (selectedDocument is null)
        {
            SelectedRule = null;
            PreviousRule = null;
            NextRule = null;
            return;
        }

        var selectedIndex = _documents.IndexOf(selectedDocument);
        SelectedRule = selectedDocument.Rule;
        PreviousRule = selectedIndex > 0 ? _documents[selectedIndex - 1].Rule : null;
        NextRule = selectedIndex < _documents.Length - 1 ? _documents[selectedIndex + 1].Rule : null;
    }

    private void ToggleNotes()
    {
        _showNotes = !_showNotes;
    }

    private static RuleDocument CreateDocument(RuleCategory category)
    {
        var title = category.GetRuleTitleUIText();
        var content = category.GetRuleUIText();
        var notes = TwilightImperiumUltimate.Web.Resources.Rules.ResourceManager.GetString(
            $"RuleCategory_{category}_Notes",
            CultureInfo.CurrentUICulture) ?? string.Empty;
        var plainTitle = ToPlainText(title);
        var firstLetter = plainTitle.FirstOrDefault(char.IsLetterOrDigit);
        var letter = firstLetter == default ? "#" : char.ToUpperInvariant(firstLetter).ToString();
        var searchText = ToPlainText($"{title} {content} {notes}");

        return new RuleDocument(
            (int)category,
            letter,
            plainTitle,
            searchText,
            new TransformedRule(category, title, content, notes));
    }

    private static ImmutableArray<RuleDocument> BuildRuleIndex() =>
        Enum.GetValues<RuleCategory>()
            .Where(category => category != RuleCategory.None)
            .Select(CreateDocument)
            .ToImmutableArray();

    private TransformedRule ToDisplayRule(RuleDocument document)
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
            return document.Rule;

        return new TransformedRule(
            document.Rule.RuleCategory,
            document.Rule.RuleTitle.HighlightSearchWord(_searchTerm, HighlightColor, false),
            document.Rule.Content,
            document.Rule.NotesContent);
    }

    private static int GetRuleId(TransformedRule rule) => (int)rule.RuleCategory;

    private string GetRulePath(TransformedRule rule) =>
        $"/rules/reference/{GetRuleId(rule)}";

    private string GetNotesToggleClass() => _showNotes
        ? "rules-notes-toggle rules-notes-toggle-on handel"
        : "rules-notes-toggle handel";

    private string GetNotesToggleText() => _showNotes ? "On" : "Off";

    private void SearchRules(string search)
    {
        _searchTerm = search;
        ApplyFilters(ActiveLetter);
        UpdateQuery();
    }

    private void SelectLetter(string letter)
    {
        ApplyFilters(letter);

        if (SelectedRule is not null)
        {
            NavigateToCatalog();
            return;
        }

        UpdateQuery();
    }

    private void NavigateToCatalog()
    {
        var uri = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri(GetRulesCatalogPath()).ToString(),
            new Dictionary<string, object?>
            {
                ["letter"] = string.IsNullOrEmpty(ActiveLetter) ? null : ActiveLetter,
            });
        NavigationManager.NavigateTo(uri);
    }

    private void UpdateQuery()
    {
        var uri = NavigationManager.GetUriWithQueryParameters(new Dictionary<string, object?>
        {
            ["search"] = string.IsNullOrWhiteSpace(_searchTerm) ? null : _searchTerm,
            ["letter"] = string.IsNullOrEmpty(ActiveLetter) ? null : ActiveLetter,
        });
        NavigationManager.NavigateTo(uri, replace: true);
    }

    private static string GetRulesCatalogPath() => "/rules/reference";

    private static string ToPlainText(string text) =>
        WebUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(text, "<.*?>", " "));

    private string GetPlainTitle(TransformedRule rule) =>
        _documents.First(document => document.Rule.RuleCategory == rule.RuleCategory).PlainTitle;

    private sealed record RuleDocument(
        int Id,
        string Letter,
        string PlainTitle,
        string SearchText,
        TransformedRule Rule);

    private sealed record TransformedRuleDocument(string Letter, TransformedRule Rule);

    private sealed record RuleSection(string Title, IReadOnlyCollection<TransformedRule> Rules);
}
