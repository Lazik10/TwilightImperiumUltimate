using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class StatisticsGrid
{
    private readonly IReadOnlyCollection<KeyValuePair<QueryLimit, string>> _queryLimitOptions =
        EnumExtensions.GetEnumValuesWithDisplayNames<QueryLimit>()
            .Where(item => item.Key != QueryLimit.None)
            .ToList();

    private readonly IReadOnlyCollection<StatisticsFilterOption> _statisticsFilters =
        EnumExtensions.GetEnumValuesWithDisplayNames<PlayerStatisticsType>()
            .Select(item => new StatisticsFilterOption(item.Key, item.Value))
            .ToList();

    private AsyncStatisticsTypeMenuItem _selectedMenuItem;
    private PlayerStatisticsType _selectedGamesType;
    private QueryLimit _selectedQueryLimit = QueryLimit.Twenty;
    private bool _prefetchStarted;

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private ILogger<StatisticsGrid> Logger { get; set; } = default!;

    private bool IsLimitHiddenStatistics =>
        _selectedMenuItem is AsyncStatisticsTypeMenuItem.General or AsyncStatisticsTypeMenuItem.History;

    private void UpdateSelectedMenuItem(AsyncStatisticsTypeMenuItem menuItem)
    {
        _selectedMenuItem = menuItem;
        StateHasChanged();
    }

    private void OnEnumChanged(PlayerStatisticsType statisticsType)
    {
        _selectedGamesType = statisticsType;
        StateHasChanged();
    }

    private void OnQueryLimitChanged(QueryLimit queryLimit)
    {
        _selectedQueryLimit = queryLimit;
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_prefetchStarted)
        {
            _prefetchStarted = true;
            await PrefetchStatisticsAsync();
        }
    }

    private int GetQueryLimit()
    {
        return _selectedQueryLimit switch
        {
            QueryLimit.Fifty => 50,
            QueryLimit.Hundred => 100,
            QueryLimit.TwoHundred => 200,
            _ => 20,
        };
    }

    private static string FormatSnapshotAge(TimeSpan age)
    {
        if (age.TotalDays >= 1)
        {
            return $"{(int)age.TotalDays}d {age.Hours}h";
        }

        if (age.TotalHours >= 1)
        {
            return $"{(int)age.TotalHours}h {age.Minutes}m";
        }

        return age.TotalMinutes >= 1
            ? $"{(int)age.TotalMinutes}m"
            : "less than 1m";
    }

    private async Task PrefetchStatisticsAsync()
    {
        using var concurrencyLimiter = new SemaphoreSlim(3);
        var limit = GetQueryLimit();
        var requests = new Func<Task>[]
        {
            () => AsyncStatsProvider.GetGeneralStatistics(),
            () => AsyncStatsProvider.GetGamesStatistics(limit),
            () => AsyncStatsProvider.GetWinsStatistics(limit),
            () => AsyncStatsProvider.GetVpStatistics(limit),
            () => AsyncStatsProvider.GetEliminationsStatistics(limit),
            () => AsyncStatsProvider.GetTurnsStatistics(limit),
            () => AsyncStatsProvider.GetDurationsStatistics(limit),
            () => AsyncStatsProvider.GetCombatStatistics(limit),
            () => AsyncStatsProvider.GetFactionsStatistics(),
            () => AsyncStatsProvider.GetOpponentsStatistics(limit),
            () => AsyncStatsProvider.GetHistoryStatistics(),
        };

        await Task.WhenAll(requests.Select(async request =>
        {
            await concurrencyLimiter.WaitAsync();
            try
            {
                await request();
            }
            catch (Exception exception)
            {
                Logger.LogDebug(exception, "Optional Async statistics prefetch failed");
            }
            finally
            {
                concurrencyLimiter.Release();
            }
        }));
    }

    private sealed record StatisticsFilterOption(PlayerStatisticsType Value, string DisplayName);
}
