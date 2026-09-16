using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Charts;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class GamesStatistics
{
    private bool _isDataLoaded;
    private AsyncGamesSummaryStatsDto _gamesSummaryStats = new AsyncGamesSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncGamesStatsDto GameStats => Filter switch
    {
        PlayerStatisticsType.All => _gamesSummaryStats.All,
        PlayerStatisticsType.Tigl => _gamesSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _gamesSummaryStats.Custom,
        _ => _gamesSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _gamesSummaryStats = await AsyncStatsProvider.GetGamesStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _gamesSummaryStats = await AsyncStatsProvider.GetGamesStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private IReadOnlyCollection<RankingBarPoint> GetMostGamesData() => PlayerRankingChartHelper.BuildPoints(
        GameStats.MostGames,
        user => user.Id,
        user => user.UserName,
        user => user.Games,
        TextColor.Yellow);

    private IReadOnlyCollection<RankingBarPoint> GetMostActiveGamesData() => PlayerRankingChartHelper.BuildPoints(
        GameStats.MostActiveGames,
        user => user.Id,
        user => user.UserName,
        user => user.Active,
        TextColor.Green);

    private void OnPlayerClick(object? tag)
    {
        if (tag is int playerId)
            NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={playerId}");
    }
}
