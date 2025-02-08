using System.Collections.Concurrent;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Web.Services.Async;

public class AsyncStatsProvider(
    ITwilightImperiumApiHttpClient httpClient)
    : IAsyncStatsProvider
{
    private static readonly int[] AllowedLimits = { 20, 50, 100, 200 };
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
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

    public async Task<AsyncGeneralSummaryStatsDto> GetGeneralStatistics()
    {
        if (_generalStats.All.Games > 0)
            return _generalStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncGeneralSummaryStatsDto>>(Paths.ApiPath_AsyncGeneralStats);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _generalStats = result.Response!.Data!;
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

        var result = await _httpClient.GetAsync<ApiResponse<AsyncGamesSummaryStatsDto>>(Paths.ApiPath_AsyncGamesStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _gamesStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncGamesSummaryStatsDto();
    }

    public async Task<AsyncWinsSummaryStatsDto> GetWinsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncWinsSummaryStatsDto();

        if (_winsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncWinsSummaryStatsDto>>(Paths.ApiPath_AsyncWinsStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _winsStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncWinsSummaryStatsDto();
    }

    public async Task<AsyncVpSummaryStatsDto> GetVpStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncVpSummaryStatsDto();

        if (_vpStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncVpSummaryStatsDto>>(Paths.ApiPath_AsyncVpStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _vpStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncVpSummaryStatsDto();
    }

    public async Task<AsyncEliminationsSummaryStatsDto> GetEliminationsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncEliminationsSummaryStatsDto();

        if (_eliminationsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncEliminationsSummaryStatsDto>>(Paths.ApiPath_AsyncEliminationsStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _eliminationsStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncEliminationsSummaryStatsDto();
    }

    public async Task<AsyncTurnsSummaryStatsDto> GetTurnsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncTurnsSummaryStatsDto();

        if (_turnsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncTurnsSummaryStatsDto>>(Paths.ApiPath_AsyncTurnsStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _turnsStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncTurnsSummaryStatsDto();
    }

    public async Task<AsyncCombatSummaryStatsDto> GetCombatStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncCombatSummaryStatsDto();

        if (_combatStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncCombatSummaryStatsDto>>(Paths.ApiPath_AsyncCombatStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _combatStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncCombatSummaryStatsDto();
    }

    public async Task<AsyncDurationsSummaryStatsDto> GetDurationsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncDurationsSummaryStatsDto();

        if (_durationsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncDurationsSummaryStatsDto>>(Paths.ApiPath_AsyncDurationsStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _durationsStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncDurationsSummaryStatsDto();
    }

    public async Task<AsyncOpponentsSummaryStatsDto> GetOpponentsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncOpponentsSummaryStatsDto();

        if (_opponentsStatsCache.TryGetValue(limit, out var cachedStats))
            return cachedStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncOpponentsSummaryStatsDto>>(Paths.ApiPath_AsyncOpponentsStats, GetLimitQuery(limit));
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _opponentsStatsCache[limit] = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncOpponentsSummaryStatsDto();
    }

    public async Task<AsyncHistorySummaryStatsDto> GetHistoryStatistics()
    {
        if (_historyStats.All.GamesHistory.Count > 0)
            return _historyStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncHistorySummaryStatsDto>>(Paths.ApiPath_AsyncHistoryStats);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _historyStats = result.Response!.Data!;
            return _historyStats;
        }

        return new AsyncHistorySummaryStatsDto();
    }

    public async Task<AsyncFactionsSummaryStatsDto> GetFactionsStatistics()
    {
        if (_factionsStats.All.Count > 0)
            return _factionsStats;

        var result = await _httpClient.GetAsync<ApiResponse<AsyncFactionsSummaryStatsDto>>(Paths.ApiPath_AsyncFactionStats);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _factionsStats = result.Response!.Data!;
            return result.Response!.Data!;
        }

        return new AsyncFactionsSummaryStatsDto();
    }

    private static string GetLimitQuery(int limit) => $"?limit={limit}";
}
