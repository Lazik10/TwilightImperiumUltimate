using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class HistoryStatistics
{
    private static readonly string[] YearColors = ["#68a9e8", "#9b7bd3", "#4fb3a4", "#d95372", "#b6a33d", "#557bb5"];

    private bool _isDataLoaded;
    private AsyncHistorySummaryStatsDto _historySummaryStats = new();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    private static int GetHistoryYear(string categoryKey) => categoryKey.StartsWith("Y-", StringComparison.Ordinal)
        ? int.Parse(categoryKey[2..], CultureInfo.InvariantCulture)
        : int.Parse(categoryKey[..4], CultureInfo.InvariantCulture);

    protected override async Task OnInitializedAsync()
    {
        _historySummaryStats = await AsyncStatsProvider.GetHistoryStatistics();
        _isDataLoaded = true;
    }

    private static string GetYearCategoryKey(int year) => $"Y-{year}";

    private static string GetMonthCategoryKey(int year, int month) =>
        $"{year:D4}{month:D2}";

    private static int GetYear<TItem>(TItem item) => item switch
    {
        AsyncGamesHistoryDto game => game.Year,
        AsyncPlayersHistoryDto player => player.Year,
        _ => throw new InvalidOperationException("Unsupported history item."),
    };

    private static int GetMonth<TItem>(TItem item) => item switch
    {
        AsyncGamesHistoryDto game => game.Month,
        AsyncPlayersHistoryDto player => player.Month,
        _ => throw new InvalidOperationException("Unsupported history item."),
    };

    private AsyncHistoryStatsDto GetSelectedHistory() => Filter switch
    {
        PlayerStatisticsType.Tigl => _historySummaryStats.Tigl,
        PlayerStatisticsType.Custom => _historySummaryStats.Custom,
        _ => _historySummaryStats.All,
    };

    private HistoryBarSeries GetGameCountsSeries() => CreateSeries(
        GetSelectedHistory().GamesHistory,
        item => item.Count);

    private HistoryBarSeries GetNewGamesSeries() => CreateSeries(
        GetSelectedHistory().GamesHistory,
        item => item.New);

    private HistoryBarSeries GetPlayerCountsSeries() => CreateSeries(
        GetSelectedHistory().PlayersHistory,
        item => item.Count);

    private HistoryBarSeries GetNewPlayerGrowthSeries() => CreateSeries(
        GetSelectedHistory().PlayersHistory,
        item => item.Growth);

    private HistoryBarSeries CreateSeries<TItem>(
        IEnumerable<TItem> source,
        Func<TItem, int> valueSelector)
        where TItem : notnull
    {
        var points = source
            .GroupBy(GetYear)
            .OrderByDescending(group => group.Key)
            .SelectMany(yearGroup => new[]
            {
                new RankingBarPoint(GetYearCategoryKey(yearGroup.Key), GetYearCategoryKey(yearGroup.Key), 0, string.Empty, null),
            }.Concat(yearGroup
                .OrderByDescending(GetMonth)
                .Select(item => new RankingBarPoint(
                    GetMonthCategoryKey(yearGroup.Key, GetMonth(item)),
                    GetMonthCategoryKey(yearGroup.Key, GetMonth(item)),
                    valueSelector(item),
                    string.Empty,
                    null))))
            .ToList();

        var fills = points
            .Select(point => GetHistoryYear(point.CategoryKey))
            .Select(year => YearColors[year % YearColors.Length])
            .ToList();

        return new HistoryBarSeries("Year", points, fills);
    }
}
