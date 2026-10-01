using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Charts;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class WinsStatistics
{
    private bool _isDataLoaded;
    private AsyncWinsSummaryStatsDto _winsSummaryStats = new AsyncWinsSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncWinsStatsDto WinsStats => Filter switch
    {
        PlayerStatisticsType.All => _winsSummaryStats.All,
        PlayerStatisticsType.Tigl => _winsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _winsSummaryStats.Custom,
        _ => _winsSummaryStats.All,
    };

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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

    private static string FormatPercentage(double value) => ((float)value).ToStringWithPrecisionAndPercentage(2);

    private IReadOnlyCollection<RankingBarPoint> GetWinsDeviationData() => PlayerRankingChartHelper.BuildPoints(
        WinsStats.AsyncWinsDeviationPlayers,
        user => user.Id,
        user => user.UserName,
        user => user.WinDeviation,
        TextColor.Green,
        user => $"{user.Games}");

    private IReadOnlyCollection<RankingBarPoint> GetWinPercentageData() => PlayerRankingChartHelper.BuildPoints(
        WinsStats.AsyncWinsPercentagePlayers,
        user => user.Id,
        user => user.UserName,
        user => user.WinPercentage,
        TextColor.Green,
        user => $"{user.Games}");

    private IReadOnlyCollection<RankingBarPoint> GetWinsData() => PlayerRankingChartHelper.BuildPoints(
        WinsStats.AsyncWinsPlayers,
        user => user.Id,
        user => user.UserName,
        user => user.Wins,
        TextColor.Green,
        user => $"{user.Games}");

    private void OnPlayerClick(object? tag)
    {
        if (tag is int playerId)
            NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={playerId}");
    }
}
