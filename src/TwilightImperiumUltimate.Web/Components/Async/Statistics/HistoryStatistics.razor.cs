using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class HistoryStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncHistorySummaryStatsDto _historySummaryStats = new AsyncHistorySummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncHistoryStatsDto HistoryStats => Filter switch
    {
        PlayerStatisticsType.All => _historySummaryStats.All,
        PlayerStatisticsType.Tigl => _historySummaryStats.Tigl,
        PlayerStatisticsType.Custom => _historySummaryStats.Custom,
        _ => _historySummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _historySummaryStats = await AsyncStatsProvider.GetHistoryStatistics();
        _isDataLoaded = true;
    }
}