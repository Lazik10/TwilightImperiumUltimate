using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class VpStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncVpSummaryStatsDto _vpSummaryStats = new AsyncVpSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public AsyncVpStatsDto VpStats => Filter switch
    {
        PlayerStatisticsType.All => _vpSummaryStats.All,
        PlayerStatisticsType.Tigl => _vpSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _vpSummaryStats.Custom,
        _ => _vpSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _vpSummaryStats = await AsyncStatsProvider.GetVpStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _vpSummaryStats = await AsyncStatsProvider.GetVpStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}