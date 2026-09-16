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
    private readonly ConcurrentDictionary<string, object> _responseCache = new();
    private readonly ConcurrentDictionary<string, string> _etags = new();
    private readonly ConcurrentDictionary<string, DateTimeOffset> _cacheTimes = new();
    private readonly ConcurrentDictionary<int, AsyncGamesSummaryStatsDto> _gamesStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncWinsSummaryStatsDto> _winsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncVpSummaryStatsDto> _vpStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncEliminationsSummaryStatsDto> _eliminationsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncTurnsSummaryStatsDto> _turnsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncCombatSummaryStatsDto> _combatStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncDurationsSummaryStatsDto> _durationsStatsCache = new();
    private readonly ConcurrentDictionary<int, AsyncOpponentsSummaryStatsDto> _opponentsStatsCache = new();
    private string? _snapshotEtag;
    private AsyncFactionsSummaryStatsDto _factionsStats = new();
    private AsyncGeneralSummaryStatsDto _generalStats = new();
    private AsyncHistorySummaryStatsDto _historyStats = new();
    private bool _hasFactionsStats;
    private bool _hasGeneralStats;
    private bool _hasHistoryStats;

    public DateTimeOffset? SnapshotGeneratedAtUtc { get; private set; }

    public TimeSpan? SnapshotAge => SnapshotGeneratedAtUtc is { } generatedAt
        ? DateTimeOffset.UtcNow - generatedAt
        : null;

    public void InvalidateCache()
    {
        _gamesStatsCache.Clear();
        _winsStatsCache.Clear();
        _vpStatsCache.Clear();
        _eliminationsStatsCache.Clear();
        _turnsStatsCache.Clear();
        _combatStatsCache.Clear();
        _durationsStatsCache.Clear();
        _opponentsStatsCache.Clear();
        _cacheTimes.Clear();
        _responseCache.Clear();
        _etags.Clear();
        _snapshotEtag = null;
        SnapshotGeneratedAtUtc = null;
        _hasFactionsStats = false;
        _hasGeneralStats = false;
        _hasHistoryStats = false;
    }

    public async Task<AsyncGeneralSummaryStatsDto> GetGeneralStatistics()
    {
        var cacheKey = GetCacheKey(nameof(GetGeneralStatistics));
        if (_hasGeneralStats && IsFresh(cacheKey))
            return _generalStats;

        var result = await GetStatisticsAsync<AsyncGeneralSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncGeneralStats);
        if (result is not null)
        {
            _generalStats = result;
            _hasGeneralStats = true;
            MarkFresh(cacheKey);
            return _generalStats;
        }

        return new AsyncGeneralSummaryStatsDto();
    }

    public async Task<AsyncGamesSummaryStatsDto> GetGamesStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncGamesSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetGamesStatistics)}:{limit}");
        if (_gamesStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncGamesSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncGamesStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _gamesStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncGamesSummaryStatsDto();
    }

    public async Task<AsyncWinsSummaryStatsDto> GetWinsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncWinsSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetWinsStatistics)}:{limit}");
        if (_winsStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncWinsSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncWinsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _winsStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncWinsSummaryStatsDto();
    }

    public async Task<AsyncVpSummaryStatsDto> GetVpStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncVpSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetVpStatistics)}:{limit}");
        if (_vpStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncVpSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncVpStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _vpStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncVpSummaryStatsDto();
    }

    public async Task<AsyncEliminationsSummaryStatsDto> GetEliminationsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncEliminationsSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetEliminationsStatistics)}:{limit}");
        if (_eliminationsStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncEliminationsSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncEliminationsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _eliminationsStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncEliminationsSummaryStatsDto();
    }

    public async Task<AsyncTurnsSummaryStatsDto> GetTurnsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncTurnsSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetTurnsStatistics)}:{limit}");
        if (_turnsStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncTurnsSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncTurnsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _turnsStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncTurnsSummaryStatsDto();
    }

    public async Task<AsyncCombatSummaryStatsDto> GetCombatStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncCombatSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetCombatStatistics)}:{limit}");
        if (_combatStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncCombatSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncCombatStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _combatStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncCombatSummaryStatsDto();
    }

    public async Task<AsyncDurationsSummaryStatsDto> GetDurationsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncDurationsSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetDurationsStatistics)}:{limit}");
        if (_durationsStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncDurationsSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncDurationsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _durationsStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncDurationsSummaryStatsDto();
    }

    public async Task<AsyncOpponentsSummaryStatsDto> GetOpponentsStatistics(int limit)
    {
        if (!AllowedLimits.Contains(limit))
            return new AsyncOpponentsSummaryStatsDto();

        var cacheKey = GetCacheKey($"{nameof(GetOpponentsStatistics)}:{limit}");
        if (_opponentsStatsCache.TryGetValue(limit, out var cachedStats) && IsFresh(cacheKey))
            return cachedStats;

        var result = await GetStatisticsAsync<AsyncOpponentsSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncOpponentsStats, GetLimitQuery(limit));
        if (result is not null)
        {
            _opponentsStatsCache[limit] = result;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncOpponentsSummaryStatsDto();
    }

    public async Task<AsyncHistorySummaryStatsDto> GetHistoryStatistics()
    {
        var cacheKey = GetCacheKey(nameof(GetHistoryStatistics));
        if (_hasHistoryStats && IsFresh(cacheKey))
            return _historyStats;

        var result = await GetStatisticsAsync<AsyncHistorySummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncHistoryStats);
        if (result is not null)
        {
            _historyStats = result;
            _hasHistoryStats = true;
            MarkFresh(cacheKey);
            return _historyStats;
        }

        return new AsyncHistorySummaryStatsDto();
    }

    public async Task<AsyncFactionsSummaryStatsDto> GetFactionsStatistics()
    {
        var cacheKey = GetCacheKey(nameof(GetFactionsStatistics));
        if (_hasFactionsStats && IsFresh(cacheKey))
            return _factionsStats;

        var result = await GetStatisticsAsync<AsyncFactionsSummaryStatsDto>(cacheKey, Paths.ApiPath_AsyncFactionStats);
        if (result is not null)
        {
            _factionsStats = result;
            _hasFactionsStats = true;
            MarkFresh(cacheKey);
            return result;
        }

        return new AsyncFactionsSummaryStatsDto();
    }

    private static string GetLimitQuery(int limit) => $"?limit={limit}";

    private string GetCacheKey(string requestKey)
    {
        var version = _snapshotEtag?.Trim('"');
        return $"{requestKey}:snapshot:{version ?? "unknown"}";
    }

    private bool IsFresh(string key)
    {
        return _cacheTimes.TryGetValue(key, out var fetchedAt)
            && DateTimeOffset.UtcNow - fetchedAt < TimeSpan.FromMinutes(15);
    }

    private void MarkFresh(string key)
    {
        _cacheTimes[key] = DateTimeOffset.UtcNow;
    }

    private async Task<T?> GetStatisticsAsync<T>(string key, string path, string query = "")
        where T : class
    {
        var request = new Lazy<Task<object?>>(
            () => FetchStatisticsAsync<T>(key, path, query),
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

    private async Task<object?> FetchStatisticsAsync<T>(string key, string path, string query)
        where T : class
    {
        var result = await _httpClient.GetWithValidationAsync<ApiResponse<T>>(path, query, _etags.GetValueOrDefault(key));
        SnapshotGeneratedAtUtc = result.SnapshotGeneratedAtUtc ?? SnapshotGeneratedAtUtc;
        if (result.StatusCode == HttpStatusCode.NotModified && _responseCache.TryGetValue(key, out var cached))
            return cached;
        if (result.StatusCode != HttpStatusCode.OK || result.Response?.Data is not T data)
            return null;

        if (!string.IsNullOrWhiteSpace(result.ETag)
            && _snapshotEtag is not null
            && !string.Equals(_snapshotEtag, result.ETag, StringComparison.Ordinal))
        {
            if (long.TryParse(result.ETag.Trim('"'), out var candidateVersion)
                && long.TryParse(_snapshotEtag.Trim('"'), out var currentVersion)
                && candidateVersion > currentVersion)
            {
                InvalidateCache();
            }
            else
            {
                return data;
            }
        }

        _snapshotEtag = result.ETag ?? _snapshotEtag;

        _responseCache[key] = data;
        if (!string.IsNullOrWhiteSpace(result.ETag))
            _etags[key] = result.ETag;

        return data;
    }
}
