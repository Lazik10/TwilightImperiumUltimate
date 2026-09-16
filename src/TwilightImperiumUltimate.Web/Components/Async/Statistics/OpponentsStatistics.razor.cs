using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Charts;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class OpponentsStatistics
{
    private bool _isDataLoaded;
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

    private IReadOnlyCollection<RankingBarPoint> GetOpponentsData() => PlayerRankingChartHelper.BuildPoints(
        OpponentsStats.PlayersWithMostOpponents,
        user => user.Id,
        user => user.UserName,
        user => user.UniqueOpponents,
        TextColor.Green,
        user => $"{user.Games}");

    private void OnPlayerClick(object? tag)
    {
        if (tag is int playerId)
            NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={playerId}");
    }
}
