namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RulesHub
{
    private string _searchValue = string.Empty;

    [Parameter]
    public string Section { get; set; } = "index";

    [Parameter]
    public string SearchWord { get; set; } = string.Empty;

    [Parameter]
    public string? Letter { get; set; }

    [Parameter]
    public string? Category { get; set; }

    [Parameter]
    public string? Source { get; set; }

    [Parameter]
    public string? Version { get; set; }

    [Parameter]
    public int? RuleId { get; set; }

    [Parameter]
    public string? ItemKey { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (!string.Equals(_searchValue, SearchWord, StringComparison.Ordinal)
            && (!string.IsNullOrWhiteSpace(SearchWord) || string.IsNullOrWhiteSpace(_searchValue)))
        {
            _searchValue = SearchWord;
        }
    }

    private void SearchAll(string search)
    {
        var normalizedSearch = search.Trim();
        _searchValue = search;

        if (normalizedSearch.Length == 1 && string.IsNullOrWhiteSpace(SearchWord))
            return;

        if (normalizedSearch.Length == 0 && string.IsNullOrWhiteSpace(SearchWord))
            return;

        var uri = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("/rules").ToString(),
            new Dictionary<string, object?>
            {
                ["search"] = normalizedSearch.Length == 0 ? null : normalizedSearch,
            });
        NavigationManager.NavigateTo(uri, replace: true);
    }
}
