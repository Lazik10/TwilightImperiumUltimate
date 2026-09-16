using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Charts;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class CombatStatistics
{
    private bool _isDataLoaded;
    private AsyncCombatSummaryStatsDto _combatSummaryStats = new AsyncCombatSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncCombatStatsDto CombatStats => Filter switch
    {
        PlayerStatisticsType.All => _combatSummaryStats.All,
        PlayerStatisticsType.Tigl => _combatSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _combatSummaryStats.Custom,
        _ => _combatSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _combatSummaryStats = await AsyncStatsProvider.GetCombatStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _combatSummaryStats = await AsyncStatsProvider.GetCombatStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private static string FormatHitsPrecision(double value) => ((float)value).ToStringWithPrecision(2);

    private static string FormatHitsDeviation(double value) => ((float)value).ToStringWithPrecisionAndPercentage(4);

    private IReadOnlyCollection<RankingBarPoint> GetTotalHitsData() => PlayerRankingChartHelper.BuildPoints(
        CombatStats.TotalHitsPlayers,
        user => user.Id,
        user => user.UserName,
        user => user.Hits,
        TextColor.Green,
        user => $"{user.Games}");

    private IReadOnlyCollection<RankingBarPoint> GetMaxHitsPerGameData() => PlayerRankingChartHelper.BuildPoints(
        CombatStats.MaxHitsPerGamePlayers,
        user => user.Id,
        user => user.UserName,
        user => user.MaxHitPerGame,
        TextColor.Green);

    private IReadOnlyCollection<RankingBarPoint> GetMaxAverageHitsPerGameData() => PlayerRankingChartHelper.BuildPoints(
        CombatStats.MaxAverageHitsPerGamePlayers,
        user => user.Id,
        user => user.UserName,
        user => user.AverageHits,
        TextColor.Green);

    private IReadOnlyCollection<RankingBarPoint> GetBestHitsDeviationData() => PlayerRankingChartHelper.BuildPoints(
        CombatStats.BestHitsDeviationPlayers,
        user => user.Id,
        user => user.UserName,
        user => user.HitsDeviation,
        TextColor.Green);

    private IReadOnlyCollection<RankingBarPoint> GetWorstHitsDeviationData() => PlayerRankingChartHelper.BuildPoints(
        CombatStats.WorstHitsDeviationPlayers,
        user => user.Id,
        user => user.UserName,
        user => Math.Abs(user.HitsDeviation),
        TextColor.Red);

    private void OnPlayerClick(object? tag)
    {
        if (tag is int playerId)
            NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={playerId}");
    }
}
