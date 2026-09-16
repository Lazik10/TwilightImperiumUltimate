using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class GeneralStatistics
{
    private bool _isDataLoaded;
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

    private static IReadOnlyCollection<RankingBarPoint> ApplyDistributionColors(
        IReadOnlyCollection<RankingBarPoint> points)
    {
        var orderedPoints = points.OrderByDescending(point => point.Value).ToList();
        var colorSlots = orderedPoints.Count switch
        {
            0 => [],
            1 => [0],
            2 => [0, 3],
            3 => [0, 1, 3],
            4 => [0, 1, 2, 3],
            5 => [0, 1, 1, 2, 3],
            _ => Enumerable.Range(0, orderedPoints.Count)
                .Select(index => Math.Clamp((int)Math.Round(index * 3d / (orderedPoints.Count - 1)), 0, 3))
                .ToList(),
        };

        var colorsByCategory = orderedPoints
            .Select((point, index) => (point.CategoryKey, Color: GetDistributionColor(colorSlots[index]).GetChartFillColor()))
            .ToDictionary(item => item.CategoryKey, item => item.Color);

        return points.Select(point => point with { Fill = colorsByCategory[point.CategoryKey] }).ToList();
    }

    private static TextColor GetDistributionColor(int colorSlot) => colorSlot switch
    {
        0 => TextColor.Green,
        1 => TextColor.Yellow,
        2 => TextColor.Orange,
        _ => TextColor.Red,
    };

    private IReadOnlyCollection<RankingBarPoint> GetTimerDistributionData() => GeneralStats.DistributionByPlayerTimers
        .OrderBy(timer => timer.Timer)
        .Select((timer, index) => new RankingBarPoint(
            index.ToString(),
            GetTimerString(timer.Timer),
            timer.Count,
            GetAverageTurnColor(timer.Timer).GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<string> GetTimerBarColors() => GetTimerDistributionData()
        .Select(point => point.Fill)
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetVpDistributionData() => GeneralStats.DistributionByVp
        .Where(vpCategory => vpCategory.Vp != -1)
        .OrderByDescending(vpCategory => vpCategory.Games)
        .Select((vpCategory, index) => new RankingBarPoint(
            index.ToString(),
            vpCategory.Vp.ToString(),
            vpCategory.Games,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList() is var points ? ApplyDistributionColors(points) : [];

    private IReadOnlyCollection<RankingBarPoint> GetPlayerCountDistributionData() => GeneralStats.DistributionByPlayerCount
        .OrderByDescending(playerCount => playerCount.PlayerCount)
        .Select((playerCount, index) => new RankingBarPoint(
            index.ToString(),
            playerCount.PlayerCount == -1 ? "Other" : playerCount.PlayerCount.ToString(),
            playerCount.Games,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList() is var points ? ApplyDistributionColors(points) : [];

    private IReadOnlyCollection<RankingBarPoint> GetAverageTurnEndDistributionData() => GeneralStats.DistributionByAverageTurnEnd
        .Where(vpCategory => vpCategory.Vp != -1)
        .OrderByDescending(vpCategory => vpCategory.Vp)
        .Select((vpCategory, index) => new RankingBarPoint(
            index.ToString(),
            vpCategory.Vp.ToString(),
            vpCategory.AverageTurnEnd,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList() is var points ? ApplyDistributionColors(points) : [];

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
