using System.Collections.Concurrent;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Web.Services.Async;

public class AsyncStatsProvider(
    ITwilightImperiumApiHttpClient httpClient)
    : IAsyncStatsProvider
{
    private static readonly int[] AllowedLimits = { 20, 50, 100, 200 };
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly ConcurrentDictionary<string, Lazy<Task<object?>>> _inFlightRequests = new();
    private readonly ConcurrentDictionary<int, AsyncGamesSummaryStatsDto> _gamesStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncWinsSummaryStatsDto> _winsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncVpSummaryStatsDto> _vpStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncEliminationsSummaryStatsDto> _eliminationsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncTurnsSummaryStatsDto> _turnsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncCombatSummaryStatsDto> _combatStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncDurationsSummaryStatsDto> _durationsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncOpponentsSummaryStatsDto> _opponentsStatsCache = new();
    private AsyncFactionsSummaryStatsDto _factionsStats = new();
    private AsyncGeneralSummaryStatsDto _generalStats = new();
    private AsyncHistorySummaryStatsDto _historyStats = new();
    private bool _hasFactionsStats;
    private bool _hasGeneralStats;
    private bool _hasHistoryStats;

    public async Task<AsyncGeneralSummaryStatsDto> GetGeneralStatistics()
    {
        if (_hasGeneralStats)
            return _generalStats;

        var result = await GetStatisticsAsync<AsyncGeneralSummaryStatsDto>(nameof(GetGeneralStatistics), Paths.ApiPath_AsyncGeneralStats);
        if (result is not null)
        {
            _generalStats = result;
            _hasGeneralStats = true;
            return _generalStats;
        }

        return new AsyncGeneralSummaryStatsDto();
    }

    public async Task<AsyncGamesSummaryStatsDto> GetGamesStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncGamesSummaryStatsDto();

        if (_gamesStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncGamesSummaryStatsDto>($"{nameof(GetGamesStatistics)}:{limit}", Paths.ApiPath_AsyncGamesStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _gamesStatsCache[limit] = result;
            return result;
        }

        return new AsyncGamesSummaryStatsDto();
    }

    public async Task<AsyncWinsSummaryStatsDto> GetWinsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncWinsSummaryStatsDto();

        if (_winsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncWinsSummaryStatsDto>($"{nameof(GetWinsStatistics)}:{limit}", Paths.ApiPath_AsyncWinsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _winsStatsCache[limit] = result;
            return result;
        }

        return new AsyncWinsSummaryStatsDto();
    }

    public async Task<AsyncVpSummaryStatsDto> GetVpStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncVpSummaryStatsDto();

        if (_vpStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncVpSummaryStatsDto>($"{nameof(GetVpStatistics)}:{limit}", Paths.ApiPath_AsyncVpStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _vpStatsCache[limit] = result;
            return result;
        }

        return new AsyncVpSummaryStatsDto();
    }

    public async Task<AsyncEliminationsSummaryStatsDto> GetEliminationsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncEliminationsSummaryStatsDto();

        if (_eliminationsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncEliminationsSummaryStatsDto>($"{nameof(GetEliminationsStatistics)}:{limit}", Paths.ApiPath_AsyncEliminationsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _eliminationsStatsCache[limit] = result;
            return result;
        }

        return new AsyncEliminationsSummaryStatsDto();
    }

    public async Task<AsyncTurnsSummaryStatsDto> GetTurnsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncTurnsSummaryStatsDto();

        if (_turnsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncTurnsSummaryStatsDto>($"{nameof(GetTurnsStatistics)}:{limit}", Paths.ApiPath_AsyncTurnsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _turnsStatsCache[limit] = result;
            return result;
        }

        return new AsyncTurnsSummaryStatsDto();
    }

    public async Task<AsyncCombatSummaryStatsDto> GetCombatStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncCombatSummaryStatsDto();

        if (_combatStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncCombatSummaryStatsDto>($"{nameof(GetCombatStatistics)}:{limit}", Paths.ApiPath_AsyncCombatStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _combatStatsCache[limit] = result;
            return result;
        }

        return new AsyncCombatSummaryStatsDto();
    }

    public async Task<AsyncDurationsSummaryStatsDto> GetDurationsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncDurationsSummaryStatsDto();

        if (_durationsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncDurationsSummaryStatsDto>($"{nameof(GetDurationsStatistics)}:{limit}", Paths.ApiPath_AsyncDurationsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _durationsStatsCache[limit] = result;
            return result;
        }

        return new AsyncDurationsSummaryStatsDto();
    }

    public async Task<AsyncOpponentsSummaryStatsDto> GetOpponentsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncOpponentsSummaryStatsDto();

        if (_opponentsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncOpponentsSummaryStatsDto>($"{nameof(GetOpponentsStatistics)}:{limit}", Paths.ApiPath_AsyncOpponentsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _opponentsStatsCache[limit] = result;
            return result;
        }

        return new AsyncOpponentsSummaryStatsDto();
    }

    public async Task<AsyncHistorySummaryStatsDto> GetHistoryStatistics()
    {
        if (_hasHistoryStats)
            return _historyStats;

        var result = await GetStatisticsAsync<AsyncHistorySummaryStatsDto>(nameof(GetHistoryStatistics), Paths.ApiPath_AsyncHistoryStats);
        if (result is not null)
        {
            _historyStats = result;
            _hasHistoryStats = true;
            return _historyStats;
        }

        return new AsyncHistorySummaryStatsDto();
    }

    public async Task<AsyncFactionsSummaryStatsDto> GetFactionsStatistics()
    {
        if (_hasFactionsStats)
            return _factionsStats;

        var result = await GetStatisticsAsync<AsyncFactionsSummaryStatsDto>(nameof(GetFactionsStatistics), Paths.ApiPath_AsyncFactionStats);
        if (result is not null)
        {
            _factionsStats = result;
            _hasFactionsStats = true;
            return result;
        }

        return new AsyncFactionsSummaryStatsDto();
    }

    private static string GetLimitQuery(int limit) => $"?limit={limit}";

    private async Task<T?> GetStatisticsAsync<T>(string key, string path, string query = "")
        where T : class
    {
        var request = new Lazy<Task<object?>>(
            () => FetchStatisticsAsync<T>(path, query),
            LazyThreadSafetyMode.ExecutionAndPublication);
        var inFlightRequest = _inFlightRequests.GetOrAdd(key, request);

        try
        {
            return (T?)await inFlightRequest.Value;
        }
        finally
        {
            if (inFlightRequest.IsValueCreated && inFlightRequest.Value.IsCompleted)
            {
                _inFlightRequests.TryRemove(new KeyValuePair<string, Lazy<Task<object?>>>(key, inFlightRequest));
            }
        }
    }

    private async Task<object?> FetchStatisticsAsync<T>(string path, string query)
        where T : class
    {
        var result = await _httpClient.GetAsync<ApiResponse<T>>(path, query);
        return result.StatusCode == HttpStatusCode.OK ? result.Response?.Data : null;
    }
}
