using TwilightImperiumUltimate.Web.Components.Shared.Bars;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.MapsArchive;

public partial class MapArchiveGrid
{
    private IReadOnlyCollection<KeyValuePair<MapTemplateFilter, string>> _mapTemplates = default!;

    private IReadOnlyCollection<KeyValuePair<FilterOrder, string>> _filterOrders = default!;

    private SearchBar _nameSearchBar = default!;

    private SearchBar _eventSearchBar = default!;

    private SearchBar _userSearchBar = default!;

    private string _mapNameSeach = string.Empty;

    private string _eventNameSearch = string.Empty;

    private string _userNameSearch = string.Empty;

    public MapTemplateFilter SelectedMapTemplate { get; set; } = MapTemplateFilter.All;

    public FilterOrder SelectedFilterOrder { get; set; } = FilterOrder.Descending;

    [Parameter]
    public IReadOnlyCollection<MapModel> AllMaps { get; set; } = new List<MapModel>();

    private IReadOnlyCollection<MapModel> FilteredMaps { get; set; } = new List<MapModel>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        InitializeEnumsWithDisplayNames();
    }

    protected override async Task OnParametersSetAsync()
    {
        FilteredMaps = AllMaps;
        await ApplyOrderFilter();
    }

    private static bool IsInvalidSearchInput(string searchKeyword)
    {
        return string.IsNullOrEmpty(searchKeyword) || string.IsNullOrWhiteSpace(searchKeyword);
    }

    private void InitializeEnumsWithDisplayNames()
    {
        _mapTemplates = EnumExtensions.GetEnumValuesWithDisplayNames<MapTemplateFilter>();
        _filterOrders = EnumExtensions.GetEnumValuesWithDisplayNames<FilterOrder>();
    }

    private void RedirectToMapDetails(int mapId)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.MapPreview}{mapId}");
    }

    private async Task ApplyMapSearchFilter()
    {
        var filteredMaps = AllMaps;

        if (!IsInvalidSearchInput(_mapNameSeach))
            filteredMaps = filteredMaps.Where(x => x.Name.Contains(_mapNameSeach, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (!IsInvalidSearchInput(_eventNameSearch))
            filteredMaps = filteredMaps.Where(x => x.EventName.Contains(_eventNameSearch, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (!IsInvalidSearchInput(_userNameSearch))
            filteredMaps = filteredMaps.Where(x => x.UserName.Contains(_userNameSearch, StringComparison.InvariantCultureIgnoreCase)).ToList();

        await ApplyMapTemplateFilter();
        await ApplyOrderFilter();

        FilteredMaps = filteredMaps;

        StateHasChanged();
    }

    private async Task ApplyMapFilter(string mapSearchKeyword)
    {
        _mapNameSeach = mapSearchKeyword;
        await ApplyMapSearchFilter();
    }

    private async Task ApplyEventFilter(string eventSearchKeyword)
    {
        _eventNameSearch = eventSearchKeyword;
        await ApplyMapSearchFilter();
    }

    private async Task ApplyUserFilter(string userSearchKeyword)
    {
        _userNameSearch = userSearchKeyword;
        await ApplyMapSearchFilter();
    }

    private Task ApplyMapTemplateFilter()
    {
        if (SelectedMapTemplate == MapTemplateFilter.All)
        {
            FilteredMaps = AllMaps;
            return Task.CompletedTask;
        }

        FilteredMaps = FilteredMaps.Where(x => x.MapTemplate == SelectedMapTemplate.GetCorrectMapTemplateFromFilter()).ToList();
        return Task.CompletedTask;
    }

    private Task ApplyOrderFilter()
    {
        if (SelectedFilterOrder == FilterOrder.Descending)
            FilteredMaps = FilteredMaps.OrderByDescending(x => x.Rating).ToList();

        if (SelectedFilterOrder == FilterOrder.Ascending)
            FilteredMaps = FilteredMaps.OrderBy(x => x.Rating).ToList();

        return Task.CompletedTask;
    }

    private void ResetSearch()
    {
        _mapNameSeach = string.Empty;
        _eventNameSearch = string.Empty;
        _userNameSearch = string.Empty;

        _nameSearchBar?.ResetSearchTerm();
        _eventSearchBar?.ResetSearchTerm();
        _userSearchBar?.ResetSearchTerm();

        SelectedFilterOrder = FilterOrder.Descending;
        SelectedMapTemplate = MapTemplateFilter.All;

        FilteredMaps = AllMaps;

        StateHasChanged();
    }
}