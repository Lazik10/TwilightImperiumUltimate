using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Charts;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class EliminationsStatistics
{
    private bool _isDataLoaded;
    private AsyncEliminationsSummaryStatsDto _eliminationsSummaryStats = new AsyncEliminationsSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncEliminationsStatsDto EliminationsStats => Filter switch
    {
        PlayerStatisticsType.All => _eliminationsSummaryStats.All,
        PlayerStatisticsType.Tigl => _eliminationsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _eliminationsSummaryStats.Custom,
        _ => _eliminationsSummaryStats.All,
    };

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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

    private static string FormatPercentage(double value) => ((float)value).ToStringWithPrecisionAndPercentage(2);

    private IReadOnlyCollection<RankingBarPoint> GetEliminationsPercentageData() => PlayerRankingChartHelper.BuildPoints(
        EliminationsStats.MostEliminationsPercentagePlayers,
        user => user.Id,
        user => user.UserName,
        user => user.EliminationsPercentage,
        TextColor.Red,
        user => $"{user.Games}");

    private IReadOnlyCollection<RankingBarPoint> GetEliminationsData() => PlayerRankingChartHelper.BuildPoints(
        EliminationsStats.MostEliminationsPlayers,
        user => user.Id,
        user => user.UserName,
        user => user.Eliminations,
        TextColor.Red,
        user => $"{user.Games}");

    private void OnPlayerClick(object? tag)
    {
        if (tag is int playerId)
            NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={playerId}");
    }
}
