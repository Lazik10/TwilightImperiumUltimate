using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class EliminationsStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncEliminationsSummaryStatsDto _eliminationsSummaryStats = new AsyncEliminationsSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public AsyncEliminationsStatsDto EliminationsStats => Filter switch
    {
        PlayerStatisticsType.All => _eliminationsSummaryStats.All,
        PlayerStatisticsType.Tigl => _eliminationsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _eliminationsSummaryStats.Custom,
        _ => _eliminationsSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _eliminationsSummaryStats = await AsyncStatsProvider.GetEliminationsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _eliminationsSummaryStats = await AsyncStatsProvider.GetEliminationsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}