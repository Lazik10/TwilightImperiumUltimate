using System.Collections;
using System.Globalization;
using System.Net;
using System.Resources;
using System.Text.RegularExpressions;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.Rules;

namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class ComponentFaqGrid
{
    private const string AllCategory = "All";
    private const string CardsCategory = "Cards";
    private const string ObjectivesCategory = "Objectives";
    private const string TechnologiesCategory = "Technologies";
    private const string RelicsCategory = "Relics";
    private const string OtherCategory = "Other";

    private static readonly ResourceManager CardNameResourceManager =
        new(Paths.ResourceNamespace_CardNames, typeof(Program).Assembly);

    private static readonly IReadOnlyList<string> CategoryOrder =
        [CardsCategory, ObjectivesCategory, TechnologiesCategory, RelicsCategory, OtherCategory];

    private static readonly IReadOnlyDictionary<string, int> CategoryOrderByName =
        CategoryOrder
            .Select((category, index) => (category, index))
            .ToDictionary(item => item.category, item => item.index, StringComparer.Ordinal);

    private static readonly IReadOnlyCollection<RulesIndexItem> ComponentFilterItems =
    [
        new(AllCategory, AllCategory),
        .. CategoryOrder.Select(category => new RulesIndexItem(category, category)),
    ];

    private static readonly IReadOnlySet<string> CardKeys = BuildKeySet<ActionCardName, AgendaCardName,
        ExplorationCardName, FrontierCardName, PromissoryNoteCardName, StrategyCardName>();

    private static readonly IReadOnlySet<string> ObjectiveKeys = BuildKeySet<ObjectiveCardName>();
    private static readonly IReadOnlySet<string> TechnologyKeys = BuildKeySet<TechnologyName>();
    private static readonly IReadOnlySet<string> RelicKeys = BuildKeySet<RelicCardName>();
    private static readonly IReadOnlySet<string> OtherKeys =
        BuildKeySet<FlagshipName, SpecialComponentName>();

    private static readonly IReadOnlySet<string> KnownComponentKeys = CardKeys
        .Concat(ObjectiveKeys)
        .Concat(TechnologyKeys)
        .Concat(RelicKeys)
        .Concat(OtherKeys)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private List<ComponentEntry> Components { get; set; } = [];
    private List<ComponentGroup> ComponentGroups { get; set; } = [];
    private IReadOnlyCollection<RulesIndexItem> LetterFilterItems { get; set; } = [];
    private IReadOnlyDictionary<string, IReadOnlyList<FaqModel>> FaqByKey { get; set; } =
        ApprovedFaqSnapshot.Empty.FaqByKey;
    private IReadOnlyList<FaqModel> SelectedFaqs { get; set; } = [];
    private ComponentEntry? SelectedComponent { get; set; }
    private string SearchTerm { get; set; } = string.Empty;
    private string ActiveCategory { get; set; } = AllCategory;
    private string ActiveLetter { get; set; } = string.Empty;
    private int FilteredComponentCount { get; set; }
    private bool _isLoading = true;
    private bool _loadFailed;

    [Parameter]
    public string SearchWord { get; set; } = string.Empty;

    [Parameter]
    public string? Category { get; set; }

    [Parameter]
    public string? ItemKey { get; set; }

    [Inject]
    private IApprovedFaqCache FaqCache { get; set; } = default!;

    [Inject]
    private IRulesCatalogCache<List<ComponentEntry>> CatalogCache { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    protected override void OnParametersSet()
    {
        if (_isLoading || Components.Count == 0)
            return;

        SyncParameters();
    }

    private async Task LoadData(bool retry = false)
    {
        _isLoading = true;
        _loadFailed = false;

        try
        {
            if (retry)
                CatalogCache.Invalidate();

            var faqSnapshot = retry
                ? await FaqCache.RetryAsync()
                : await FaqCache.GetAsync();

            FaqByKey = faqSnapshot.FaqByKey;
            Components = CatalogCache.GetOrCreate(
                CultureInfo.CurrentUICulture.Name,
                FaqCache.Version,
                () => BuildComponentIndex(faqSnapshot));
            SyncParameters();
        }
        catch
        {
            _loadFailed = true;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private Task RetryLoad() => LoadData(retry: true);

    private void SyncParameters()
    {
        SearchTerm = SearchWord;
        ActiveCategory = string.IsNullOrWhiteSpace(SearchTerm)
            ? NormalizeCategory(Category ?? AllCategory)
            : AllCategory;
        ActiveLetter = string.Empty;
        ApplyFilters();
        SetSelectedComponent();
    }

    private void SearchComponents(string search)
    {
        SearchTerm = search;
        ActiveCategory = AllCategory;
        ActiveLetter = string.Empty;
        ApplyFilters();
        UpdateQuery(returnToCatalog:
            SelectedComponent is not null && !string.IsNullOrWhiteSpace(SearchTerm));
    }

    private void SelectCategory(string category)
    {
        ActiveCategory = NormalizeCategory(category);
        ActiveLetter = string.Empty;
        ApplyFilters();
        UpdateQuery(returnToCatalog: SelectedComponent is not null);
    }

    private void SelectLetter(string letter)
    {
        ActiveLetter = letter;
        ApplyFilters();
        UpdateQuery();
    }

    private void ApplyFilters()
    {
        var categoryMatches = Components
            .Where(component => ActiveCategory == AllCategory || component.Category == ActiveCategory)
            .Where(component => string.IsNullOrWhiteSpace(SearchTerm)
                || component.SearchText.Contains(SearchTerm, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        var availableLetters = categoryMatches.Select(component => component.Letter).ToHashSet();
        LetterFilterItems = RulesAlphabet.Build(availableLetters);
        ActiveLetter = RulesAlphabet.Normalize(ActiveLetter, availableLetters);
        var filteredComponents = string.IsNullOrEmpty(ActiveLetter)
            ? categoryMatches
            : categoryMatches
                .Where(component => component.Letter == ActiveLetter)
                .ToList();

        FilteredComponentCount = filteredComponents.Count;
        ComponentGroups = filteredComponents
            .GroupBy(component => component.Category)
            .OrderBy(group => GetCategoryOrder(group.Key))
            .Select(group => new ComponentGroup(group.Key, group.ToList()))
            .ToList();
    }

    private void SetSelectedComponent()
    {
        SelectedComponent = string.IsNullOrWhiteSpace(ItemKey)
            ? null
            : Components.FirstOrDefault(component =>
                component.Key.Equals(ItemKey, StringComparison.OrdinalIgnoreCase));

        SelectedFaqs = SelectedComponent is not null
            && FaqByKey.TryGetValue(SelectedComponent.Key, out var faqs)
                ? faqs
                : [];
    }

    private static List<ComponentEntry> BuildComponentIndex(ApprovedFaqSnapshot faqSnapshot)
    {
        var notesByKey = GetNotesByKey();
        var keys = notesByKey.Keys
            .Concat(faqSnapshot.FaqByKey.Keys.Where(KnownComponentKeys.Contains))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        return keys
            .Select(key =>
            {
                notesByKey.TryGetValue(key, out var notesHtml);
                faqSnapshot.SearchTextByKey.TryGetValue(key, out var faqText);

                var category = GetCategory(key);
                var title = GetTitle(key);
                var firstLetter = title.FirstOrDefault(char.IsLetterOrDigit);
                var letter = firstLetter == default ? "#" : char.ToUpperInvariant(firstLetter).ToString();
                var notesText = StripMarkup(notesHtml ?? string.Empty);
                var searchText = string.Join(' ', title, category, notesText, faqText ?? string.Empty);
                var hasContent = !string.IsNullOrWhiteSpace(notesText)
                    || faqSnapshot.FaqByKey.ContainsKey(key);

                return hasContent
                    ? new ComponentEntry(
                    key,
                    title,
                    letter,
                    category,
                    notesHtml ?? string.Empty,
                    searchText)
                    : null;
            })
            .OfType<ComponentEntry>()
            .OrderBy(component => GetCategoryOrder(component.Category))
            .ThenBy(component => component.Title)
            .ToList();
    }

    private static Dictionary<string, string> GetNotesByKey()
    {
        var resourceSet = RuleNotes.ResourceManager.GetResourceSet(
            CultureInfo.CurrentUICulture,
            createIfNotExists: true,
            tryParents: true);
        var notesByKey = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (resourceSet is null)
            return notesByKey;

        foreach (DictionaryEntry entry in resourceSet)
        {
            if (entry.Key is string key && entry.Value is string value)
                notesByKey[key] = value;
        }

        return notesByKey;
    }

    private string GetCatalogPath() => BuildPath("/rules/components");

    private string GetComponentPath(ComponentEntry component) =>
        BuildPath($"/rules/components/{Uri.EscapeDataString(component.Key)}");

    private string BuildPath(string path)
    {
        var query = new List<string>();
        if (ActiveCategory != AllCategory)
            query.Add($"category={Uri.EscapeDataString(ActiveCategory)}");
        return query.Count == 0 ? path : $"{path}?{string.Join('&', query)}";
    }

    private void UpdateQuery(bool returnToCatalog = false)
    {
        var query = new Dictionary<string, object?>
        {
            ["search"] = string.IsNullOrWhiteSpace(SearchTerm) ? null : SearchTerm,
            ["category"] = string.IsNullOrWhiteSpace(SearchTerm)
                && ActiveCategory != AllCategory
                    ? ActiveCategory
                    : null,
            ["letter"] = null,
        };
        var uri = returnToCatalog
            ? NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri("/rules/components").ToString(),
                query)
            : NavigationManager.GetUriWithQueryParameters(query);
        NavigationManager.NavigateTo(uri, replace: true);
    }

    private static string NormalizeCategory(string category) =>
        category.Equals(AllCategory, StringComparison.OrdinalIgnoreCase)
            ? AllCategory
            : CategoryOrder.FirstOrDefault(item =>
                item.Equals(category, StringComparison.OrdinalIgnoreCase)) ?? AllCategory;

    private static string GetCategory(string key)
    {
        if (CardKeys.Contains(key))
            return CardsCategory;
        if (ObjectiveKeys.Contains(key))
            return ObjectivesCategory;
        if (TechnologyKeys.Contains(key))
            return TechnologiesCategory;
        if (RelicKeys.Contains(key))
            return RelicsCategory;
        if (OtherKeys.Contains(key))
            return OtherCategory;
        return OtherCategory;
    }

    private static string GetTitle(string key)
    {
        if (TryGetDisplayName<ActionCardName>(key, out var title)
            || TryGetDisplayName<AgendaCardName>(key, out title)
            || TryGetDisplayName<ExplorationCardName>(key, out title)
            || TryGetDisplayName<FrontierCardName>(key, out title)
            || TryGetDisplayName<ObjectiveCardName>(key, out title)
            || TryGetDisplayName<PromissoryNoteCardName>(key, out title)
            || TryGetDisplayName<RelicCardName>(key, out title)
            || TryGetDisplayName<StrategyCardName>(key, out title)
            || TryGetDisplayName<TechnologyName>(key, out title)
            || TryGetDisplayName<FlagshipName>(key, out title)
            || TryGetDisplayName<SpecialComponentName>(key, out title))
        {
            return title;
        }

        return Humanize(key);
    }

    private static bool TryGetDisplayName<TEnum>(string key, out string title)
        where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(key, ignoreCase: true, out var value))
        {
            var resourceKey = $"{typeof(TEnum).Name}_{value}";
            title = CardNameResourceManager.GetString(
                resourceKey,
                CultureInfo.CurrentUICulture) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(title))
                return true;
        }

        title = string.Empty;
        return false;
    }

    private static IReadOnlySet<string> BuildKeySet<TEnum>()
        where TEnum : struct, Enum =>
        Enum.GetNames<TEnum>().ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static IReadOnlySet<string> BuildKeySet<TEnum1, TEnum2>()
        where TEnum1 : struct, Enum
        where TEnum2 : struct, Enum =>
        BuildKeySet<TEnum1>().Concat(BuildKeySet<TEnum2>())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static IReadOnlySet<string> BuildKeySet<TEnum1, TEnum2, TEnum3, TEnum4, TEnum5, TEnum6>()
        where TEnum1 : struct, Enum
        where TEnum2 : struct, Enum
        where TEnum3 : struct, Enum
        where TEnum4 : struct, Enum
        where TEnum5 : struct, Enum
        where TEnum6 : struct, Enum =>
        BuildKeySet<TEnum1, TEnum2>()
            .Concat(BuildKeySet<TEnum3>())
            .Concat(BuildKeySet<TEnum4>())
            .Concat(BuildKeySet<TEnum5>())
            .Concat(BuildKeySet<TEnum6>())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static int GetCategoryOrder(string category) =>
        CategoryOrderByName.GetValueOrDefault(category, CategoryOrder.Count);

    private static MarkupString GetComponentNotes(ComponentEntry component) =>
        (MarkupString)component.NotesHtml;

    private static string StripMarkup(string value) =>
        WebUtility.HtmlDecode(HtmlTagRegex().Replace(value, " "));

    private static string Humanize(string value) =>
        HumanizeRegex().Replace(value.Replace("Omega", " Omega"), "$1 $2");

    [GeneratedRegex("<.*?>", RegexOptions.Singleline)]
    private static partial Regex HtmlTagRegex();

    [GeneratedRegex("([a-z0-9])([A-Z])")]
    private static partial Regex HumanizeRegex();

    private sealed record ComponentEntry(
        string Key,
        string Title,
        string Letter,
        string Category,
        string NotesHtml,
        string SearchText);

    private sealed record ComponentGroup(string Title, IReadOnlyCollection<ComponentEntry> Items);
}
