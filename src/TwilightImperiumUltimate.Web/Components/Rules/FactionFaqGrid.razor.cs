using Microsoft.JSInterop;
using System.Globalization;
using TwilightImperiumUltimate.Web.Services.Rules;

namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class FactionFaqGrid : IAsyncDisposable
{
    private const string OfficialSources = "official";
    private const string UnofficialSources = "unofficial";

    private static readonly IReadOnlyCollection<RulesIndexItem> SourceFilters =
    [
        new(OfficialSources, "Official"),
        new(UnofficialSources, "Unofficial"),
    ];

    private List<FactionEntry> Factions { get; set; } = [];
    private List<FactionSection> FactionSections { get; set; } = [];
    private IReadOnlyCollection<RulesIndexItem> VersionFilterItems { get; set; } = [];
    private FactionEntry? SelectedFaction { get; set; }
    private string SearchTerm { get; set; } = string.Empty;
    private string ActiveSourceFilter { get; set; } = OfficialSources;
    private string ActiveGameVersion { get; set; } = string.Empty;
    private int FilteredFactionCount { get; set; }
    private bool _isLoading = true;
    private bool _loadFailed;
    private ElementReference _detailElement;
    private IJSObjectReference? _indexModule;
    private string? _indexedFactionKey;

    [Parameter]
    public string SearchWord { get; set; } = string.Empty;

    [Parameter]
    public string? Source { get; set; }

    [Parameter]
    public string? Version { get; set; }

    [Parameter]
    public string? ItemKey { get; set; }

    [Parameter]
    public bool CompactSearchResults { get; set; }

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IApprovedFaqCache FaqCache { get; set; } = default!;

    [Inject]
    private IRulesCatalogCache<List<FactionEntry>> CatalogCache { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    protected override void OnParametersSet()
    {
        if (Factions.Count == 0)
            return;

        SyncStateFromParameters();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (SelectedFaction is null || SelectedFaction.Key == _indexedFactionKey)
            return;

        try
        {
            _indexModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./Components/Rules/FactionFaqGrid.razor.js");
            await _indexModule.InvokeVoidAsync("buildFactionIndex", _detailElement);
            _indexedFactionKey = SelectedFaction.Key;
        }
        catch (JSException)
        {
            // The notes remain usable when progressive enhancement is unavailable.
        }
    }

    private async Task LoadData(bool forceReload = false)
    {
        _isLoading = true;
        _loadFailed = false;

        try
        {
            if (forceReload)
                CatalogCache.Invalidate();

            var faqsTask = forceReload
                ? FaqCache.RetryAsync()
                : FaqCache.GetAsync();
            Factions = await CatalogCache.GetOrCreateAsync(
                CultureInfo.CurrentUICulture.Name,
                FaqCache.Version,
                () => LoadFactionEntries(faqsTask));
            SyncStateFromParameters();
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

    private Task RetryLoad() => LoadData(forceReload: true);

    private async Task<List<FactionEntry>> LoadFactionEntries(
        Task<ApprovedFaqSnapshot> faqsTask)
    {
        var factionsTask = HttpClient.GetAsync<ApiResponse<ItemListDto<FactionDto>>>(Paths.ApiPath_Factions);
        await Task.WhenAll(factionsTask, faqsTask);

        var factionsResult = await factionsTask;
        if (factionsResult.StatusCode != HttpStatusCode.OK
            || factionsResult.Response?.Data?.Items is null)
        {
            throw new InvalidOperationException("Faction data could not be loaded.");
        }

        var factionModels = Mapper.Map<List<FactionModel>>(factionsResult.Response.Data.Items);
        return BuildFactionEntries(factionModels, (await faqsTask).FaqByKey);
    }

    private void SyncStateFromParameters()
    {
        SearchTerm = SearchWord;
        ActiveSourceFilter = NormalizeSourceFilter(Source);
        ActiveGameVersion = string.IsNullOrWhiteSpace(SearchTerm)
            ? NormalizeGameVersion(Version)
            : string.Empty;
        SyncSourceWithGameVersion();
        BuildVersionFilterItems();
        ApplySearch();
        SetSelectedFaction();
    }

    private static List<FactionEntry> BuildFactionEntries(
        IReadOnlyCollection<FactionModel> factionModels,
        IReadOnlyDictionary<string, IReadOnlyList<FaqModel>> faqsByFaction)
    {
        return factionModels
            .Select(model => BuildFactionEntry(
                model,
                faqsByFaction.GetValueOrDefault(model.FactionName.ToString()) ?? []))
            .OrderBy(entry => entry.Title)
            .ToList();
    }

    private static FactionEntry BuildFactionEntry(FactionModel model, IReadOnlyList<FaqModel> faqs)
    {
        var key = model.FactionName.ToString();
        var title = model.FactionName.GetFactionUIText(FactionResourceType.Title);
        var notesHtml = FactionsInfo.ResourceManager.GetString(
            $"{model.FactionName}_{FactionResourceType.Notes}",
            CultureInfo.CurrentUICulture) ?? string.Empty;
        var faqText = string.Join(' ', faqs.Select(faq =>
            $"{faq.QuestionEnglish} {faq.AnswerEnglish} {faq.QuestionCzech} {faq.AnswerCzech}"));
        var searchText = $"{title} {notesHtml} {faqText}";

        return new FactionEntry(
            key,
            title,
            searchText,
            model.GameVersion != GameVersion.DiscordantStars,
            model.GameVersion,
            faqs,
            notesHtml);
    }

    private void SelectSourceFilter(string source)
    {
        ActiveSourceFilter = NormalizeSourceFilter(source);
        ActiveGameVersion = string.Empty;
        BuildVersionFilterItems();
        ApplySearch();
        UpdateQuery();
    }

    private void SelectGameVersion(string gameVersion)
    {
        ActiveGameVersion = NormalizeGameVersion(gameVersion);
        if (Enum.TryParse<GameVersion>(ActiveGameVersion, out var parsed))
        {
            ActiveSourceFilter = parsed == GameVersion.DiscordantStars
                ? UnofficialSources
                : OfficialSources;
        }

        BuildVersionFilterItems();
        ApplySearch();
        UpdateQuery();
    }

    private void ApplySearch()
    {
        var sourceMatches = string.IsNullOrWhiteSpace(SearchTerm)
            ? Factions.Where(MatchesSourceFilter)
            : Factions;
        var filteredFactions = sourceMatches
            .Where(faction => string.IsNullOrEmpty(ActiveGameVersion)
                || faction.Version.ToString() == ActiveGameVersion)
            .Where(faction => string.IsNullOrWhiteSpace(SearchTerm)
                || faction.SearchText.Contains(SearchTerm, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        FilteredFactionCount = filteredFactions.Count;
        FactionSections = filteredFactions
            .GroupBy(faction => faction.Version)
            .OrderBy(group => GetGameVersionOrder(group.Key))
            .Select(group => new FactionSection(
                group.Key,
                GetGameVersionTitle(group.Key),
                group.ToList()))
            .ToList();
    }

    private void BuildVersionFilterItems()
    {
        VersionFilterItems = Factions
            .Where(MatchesSourceFilter)
            .Select(faction => faction.Version)
            .Distinct()
            .OrderBy(GetGameVersionOrder)
            .Select(version => new RulesIndexItem(
                version.ToString(),
                GetGameVersionTitle(version)))
            .ToList();
    }

    private void SetSelectedFaction()
    {
        SelectedFaction = string.IsNullOrWhiteSpace(ItemKey)
            ? null
            : Factions.FirstOrDefault(faction =>
                faction.Key.Equals(ItemKey, StringComparison.OrdinalIgnoreCase));
    }

    private void UpdateQuery()
    {
        var query = new Dictionary<string, object?>
        {
            ["source"] = ActiveSourceFilter,
            ["version"] = string.IsNullOrEmpty(ActiveGameVersion) ? null : ActiveGameVersion,
        };
        var uri = NavigationManager.GetUriWithQueryParameters(query);
        NavigationManager.NavigateTo(uri, replace: true);
    }

    private string GetCatalogPath() => BuildPath("/rules/factions");

    private string GetFactionPath(FactionEntry faction) =>
        BuildPath($"/rules/factions/{Uri.EscapeDataString(faction.Key)}");

    private static string GetFactionIconPath(FactionEntry faction) =>
        $"/resources/images/shared/factionicons/{faction.Key}.webp";

    private string BuildPath(string path)
    {
        var query = new List<string>();
        query.Add($"source={Uri.EscapeDataString(ActiveSourceFilter)}");
        if (!string.IsNullOrEmpty(ActiveGameVersion))
            query.Add($"version={Uri.EscapeDataString(ActiveGameVersion)}");
        return query.Count == 0 ? path : $"{path}?{string.Join('&', query)}";
    }

    private string NormalizeGameVersion(string? gameVersion) =>
        Enum.TryParse<GameVersion>(gameVersion, true, out var parsed)
            && Factions.Any(faction => faction.Version == parsed)
                ? parsed.ToString()
                : string.Empty;

    private void SyncSourceWithGameVersion()
    {
        if (!Enum.TryParse<GameVersion>(ActiveGameVersion, out var gameVersion))
            return;

        ActiveSourceFilter = gameVersion == GameVersion.DiscordantStars
            ? UnofficialSources
            : OfficialSources;
    }

    private static string NormalizeSourceFilter(string? source) => source switch
    {
        UnofficialSources => UnofficialSources,
        _ => OfficialSources,
    };

    private bool MatchesSourceFilter(FactionEntry faction) => ActiveSourceFilter switch
    {
        OfficialSources => faction.IsOfficial,
        _ => !faction.IsOfficial,
    };

    private static int GetGameVersionOrder(GameVersion gameVersion) => gameVersion switch
    {
        GameVersion.BaseGame => 0,
        GameVersion.ProphecyOfKings => 1,
        GameVersion.CodexVigil => 2,
        GameVersion.ThundersEdge => 3,
        GameVersion.DiscordantStars => 4,
        _ => 5,
    };

    private static string GetGameVersionTitle(GameVersion gameVersion) => gameVersion switch
    {
        GameVersion.BaseGame => Strings.GameVersion_BaseGame,
        GameVersion.ProphecyOfKings => Strings.GameVersion_ProphecyOfKings,
        GameVersion.CodexVigil => Strings.GameVersion_CodexVigil,
        GameVersion.ThundersEdge => Strings.GameVersion_ThundersEdge,
        GameVersion.DiscordantStars => Strings.GameVersion_DiscordantStars,
        _ => gameVersion.ToString(),
    };

    private sealed record FactionEntry(
        string Key,
        string Title,
        string SearchText,
        bool IsOfficial,
        GameVersion Version,
        IReadOnlyList<FaqModel> Faqs,
        string DetailNotesHtml);

    private sealed record FactionSection(
        GameVersion GameVersion,
        string Title,
        IReadOnlyCollection<FactionEntry> Items);

    public async ValueTask DisposeAsync()
    {
        if (_indexModule is not null)
            await _indexModule.DisposeAsync();
    }
}
