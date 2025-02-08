using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class WinsStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncWinsSummaryStatsDto _winsSummaryStats = new AsyncWinsSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public AsyncWinsStatsDto WinsStats => Filter switch
    {
        PlayerStatisticsType.All => _winsSummaryStats.All,
        PlayerStatisticsType.Tigl => _winsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _winsSummaryStats.Custom,
        _ => _winsSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _winsSummaryStats = await AsyncStatsProvider.GetWinsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _winsSummaryStats = await AsyncStatsProvider.GetWinsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}