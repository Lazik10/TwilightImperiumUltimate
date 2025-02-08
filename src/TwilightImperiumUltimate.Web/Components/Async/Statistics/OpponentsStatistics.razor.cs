using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class OpponentsStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncOpponentsSummaryStatsDto _opponentsSummaryStats = new AsyncOpponentsSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncOpponentsStatsDto OpponentsStats => Filter switch
    {
        PlayerStatisticsType.All => _opponentsSummaryStats.All,
        PlayerStatisticsType.Tigl => _opponentsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _opponentsSummaryStats.Custom,
        _ => _opponentsSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _opponentsSummaryStats = await AsyncStatsProvider.GetOpponentsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _opponentsSummaryStats = await AsyncStatsProvider.GetOpponentsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}