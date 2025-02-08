using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class GeneralStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncGeneralSummaryStatsDto _generalSummaryStats = new AsyncGeneralSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    public AsyncGeneralStatsDto GeneralStats => Filter switch
    {
        PlayerStatisticsType.All => _generalSummaryStats.All,
        PlayerStatisticsType.Tigl => _generalSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _generalSummaryStats.Custom,
        _ => _generalSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _generalSummaryStats = await AsyncStatsProvider.GetGeneralStatistics();
        _isDataLoaded = true;
    }

    private TextColor GetAverageTurnColor(int timerCategory)
    {
        return timerCategory switch
        {
            > 6 => TextColor.Red,
            > 4 => TextColor.Orange,
            > 2 => TextColor.Yellow,
            _ => TextColor.Green,
        };
    }

    private string GetTimerString(int timerCategory)
    {
        return timerCategory switch
        {
            9 => "> 8h",
            8 => "7h - 8h",
            7 => "6h - 7h",
            6 => "5h - 6h",
            5 => "4h - 5h",
            4 => "3h - 4h",
            3 => "2h - 3h",
            2 => "1h - 2h",
            1 => "< 1h",
            _ => "< 30m",
        };
    }
}