using TwilightImperiumUltimate.Web.Components.Shared.Bars;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Models.SlicesArchive;

namespace TwilightImperiumUltimate.Web.Components.SlicesArchive;

public partial class SlicesArchiveGrid
{
    private IReadOnlyCollection<KeyValuePair<SliceCountFilter, string>> _sliceFilters = default!;

    private IReadOnlyCollection<KeyValuePair<FilterOrder, string>> _filterOrders = default!;

    private SearchBar _nameSearchBar = default!;

    private SearchBar _eventSearchBar = default!;

    private SearchBar _userSearchBar = default!;

    private string _nameSearch = string.Empty;

    private string _eventNameSearch = string.Empty;

    private string _userNameSearch = string.Empty;

    public SliceCountFilter SelectedSliceFilter { get; set; } = SliceCountFilter.All;

    public FilterOrder SelectedFilterOrder { get; set; } = FilterOrder.Descending;

    [Parameter]
    public IReadOnlyCollection<SliceDraftModel> AllSliceDrafts { get; set; } = new List<SliceDraftModel>();

    private IReadOnlyCollection<SliceDraftModel> FilteredSliceDrafts { get; set; } = new List<SliceDraftModel>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        InitializeEnumsWithDisplayNames();
    }

    protected override async Task OnParametersSetAsync()
    {
        FilteredSliceDrafts = AllSliceDrafts;
        await ApplyOrderFilter();
    }

    private static bool IsInvalidSearchInput(string searchKeyword)
    {
        return string.IsNullOrEmpty(searchKeyword) || string.IsNullOrWhiteSpace(searchKeyword);
    }

    private void InitializeEnumsWithDisplayNames()
    {
        _sliceFilters = EnumExtensions.GetEnumValuesWithDisplayNames<SliceCountFilter>();
        _filterOrders = EnumExtensions.GetEnumValuesWithDisplayNames<FilterOrder>();
    }

    private void RedirectToMapDetails(int sliceDraftId)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.SliceDraftPreview}{sliceDraftId}");
    }

    private async Task ApplySliceDraftSearchFilter()
    {
        var filteredSliceDrafts = AllSliceDrafts;

        if (!IsInvalidSearchInput(_nameSearch))
            filteredSliceDrafts = filteredSliceDrafts.Where(x => x.Name.Contains(_nameSearch, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (!IsInvalidSearchInput(_eventNameSearch))
            filteredSliceDrafts = filteredSliceDrafts.Where(x => x.EventName.Contains(_eventNameSearch, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (!IsInvalidSearchInput(_userNameSearch))
            filteredSliceDrafts = filteredSliceDrafts.Where(x => x.UserName.Contains(_userNameSearch, StringComparison.InvariantCultureIgnoreCase)).ToList();

        await ApplySliceCountFilter();
        await ApplyOrderFilter();

        FilteredSliceDrafts = filteredSliceDrafts;

        StateHasChanged();
    }

    private async Task ApplyNameFilter(string nameSearchKeyword)
    {
        _nameSearch = nameSearchKeyword;
        await ApplySliceDraftSearchFilter();
    }

    private async Task ApplyEventFilter(string eventSearchKeyword)
    {
        _eventNameSearch = eventSearchKeyword;
        await ApplySliceDraftSearchFilter();
    }

    private async Task ApplyUserFilter(string userSearchKeyword)
    {
        _userNameSearch = userSearchKeyword;
        await ApplySliceDraftSearchFilter();
    }

    private Task ApplySliceCountFilter()
    {
        if (SelectedSliceFilter == SliceCountFilter.All)
        {
            FilteredSliceDrafts = AllSliceDrafts;
            return Task.CompletedTask;
        }

        var count = GetSliceCountFromFilter();
        FilteredSliceDrafts = FilteredSliceDrafts.Where(x => x.SliceCount == count).ToList();
        return Task.CompletedTask;
    }

    private int GetSliceCountFromFilter()
    {
        return SelectedSliceFilter switch
        {
            SliceCountFilter.ThreeSlices => 3,
            SliceCountFilter.FourSlices => 4,
            SliceCountFilter.FiveSlices => 5,
            SliceCountFilter.SixSlices => 6,
            SliceCountFilter.SevenSlices => 7,
            SliceCountFilter.EightSlices => 8,
            _ => 0,
        };
    }

    private Task ApplyOrderFilter()
    {
        if (SelectedFilterOrder == FilterOrder.Descending)
            FilteredSliceDrafts = FilteredSliceDrafts.OrderByDescending(x => x.Rating).ToList();

        if (SelectedFilterOrder == FilterOrder.Ascending)
            FilteredSliceDrafts = FilteredSliceDrafts.OrderBy(x => x.Rating).ToList();

        return Task.CompletedTask;
    }

    private void ResetSearch()
    {
        _nameSearch = string.Empty;
        _eventNameSearch = string.Empty;
        _userNameSearch = string.Empty;

        _nameSearchBar?.ResetSearchTerm();
        _eventSearchBar?.ResetSearchTerm();
        _userSearchBar?.ResetSearchTerm();

        SelectedFilterOrder = FilterOrder.Descending;
        SelectedSliceFilter = SliceCountFilter.All;

        FilteredSliceDrafts = AllSliceDrafts;

        StateHasChanged();
    }
}