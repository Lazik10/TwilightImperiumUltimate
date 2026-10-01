using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Charts;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class VpStatistics
{
    private bool _isDataLoaded;
    private AsyncVpSummaryStatsDto _vpSummaryStats = new AsyncVpSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncVpStatsDto VpStats => Filter switch
    {
        PlayerStatisticsType.All => _vpSummaryStats.All,
        PlayerStatisticsType.Tigl => _vpSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _vpSummaryStats.Custom,
        _ => _vpSummaryStats.All,
    };

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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

    private static string FormatPercentage(double value) => ((float)value).ToStringWithPrecisionAndPercentage(2);

    private IReadOnlyCollection<RankingBarPoint> GetVpPercentageData() => PlayerRankingChartHelper.BuildPoints(
        VpStats.VpPercentagesPlayers,
        user => user.Id,
        user => user.UserName,
        user => user.VpPercentage,
        TextColor.Green,
        user => $"{user.Games}");

    private IReadOnlyCollection<RankingBarPoint> GetMostVpData() => PlayerRankingChartHelper.BuildPoints(
        VpStats.MostVpPlayers,
        user => user.Id,
        user => user.UserName,
        user => user.Vp,
        TextColor.Yellow,
        user => $"{user.Games}");

    private void OnPlayerClick(object? tag)
    {
        if (tag is int playerId)
            NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={playerId}");
    }
}
