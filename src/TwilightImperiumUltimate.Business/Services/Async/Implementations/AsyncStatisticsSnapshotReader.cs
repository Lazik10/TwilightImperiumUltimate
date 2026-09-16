using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public sealed class AsyncStatisticsSnapshotReader(
    IAsyncStatisticsSnapshotRepository repository,
    IMemoryCache memoryCache,
    IDistributedCache distributedCache,
    ILogger<AsyncStatisticsSnapshotReader> logger)
    : IAsyncStatisticsSnapshotReader
{
    private const int CanonicalLimit = 200;
    private const string MemoryCacheKey = "async-statistics-snapshot:current";
    private const string DistributedCacheKeyPrefix = "async-statistics-snapshot:v";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(65);
    private static readonly SemaphoreSlim CachePopulationLock = new(1, 1);

    private readonly IAsyncStatisticsSnapshotRepository _repository = repository;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly ILogger<AsyncStatisticsSnapshotReader> _logger = logger;

    public void Invalidate()
    {
        _memoryCache.Remove(MemoryCacheKey);
    }

    public async Task<AsyncGeneralSummaryStatsDto> GetGeneralAsync(CancellationToken cancellationToken)
    {
        return (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.General), "all", null, cancellationToken)).Payload.General;
    }

    public async Task<AsyncGamesSummaryStatsDto> GetGamesAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Games), "all", limit, cancellationToken)).Payload.Games;
        return new AsyncGamesSummaryStatsDto(
            new AsyncGamesStatsDto(stats.All.MostGames.Take(limit).ToList(), stats.All.MostActiveGames.Take(limit).ToList()),
            new AsyncGamesStatsDto(stats.Tigl.MostGames.Take(limit).ToList(), stats.Tigl.MostActiveGames.Take(limit).ToList()),
            new AsyncGamesStatsDto(stats.Custom.MostGames.Take(limit).ToList(), stats.Custom.MostActiveGames.Take(limit).ToList()));
    }

    public async Task<AsyncWinsSummaryStatsDto> GetWinsAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Wins), "all", limit, cancellationToken)).Payload.Wins;
        return new AsyncWinsSummaryStatsDto(
            SliceWins(stats.All, limit), SliceWins(stats.Tigl, limit), SliceWins(stats.Custom, limit));
    }

    public async Task<AsyncVpSummaryStatsDto> GetVictoryPointsAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.VictoryPoints), "all", limit, cancellationToken)).Payload.VictoryPoints;
        return new AsyncVpSummaryStatsDto(
            SliceVp(stats.All, limit), SliceVp(stats.Tigl, limit), SliceVp(stats.Custom, limit));
    }

    public async Task<AsyncEliminationsSummaryStatsDto> GetEliminationsAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Eliminations), "all", limit, cancellationToken)).Payload.Eliminations;
        return new AsyncEliminationsSummaryStatsDto(
            SliceEliminations(stats.All, limit), SliceEliminations(stats.Tigl, limit), SliceEliminations(stats.Custom, limit));
    }

    public async Task<AsyncTurnsSummaryStatsDto> GetTurnsAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Turns), "all", limit, cancellationToken)).Payload.Turns;
        return new AsyncTurnsSummaryStatsDto(
            SliceTurns(stats.All, limit), SliceTurns(stats.Tigl, limit), SliceTurns(stats.Custom, limit));
    }

    public async Task<AsyncCombatSummaryStatsDto> GetCombatAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Combat), "all", limit, cancellationToken)).Payload.Combat;
        return new AsyncCombatSummaryStatsDto(
            SliceCombat(stats.All, limit), SliceCombat(stats.Tigl, limit), SliceCombat(stats.Custom, limit));
    }

    public async Task<AsyncDurationsSummaryStatsDto> GetDurationsAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Durations), "all", limit, cancellationToken)).Payload.Durations;
        return new AsyncDurationsSummaryStatsDto(
            SliceDurations(stats.All, limit), SliceDurations(stats.Tigl, limit), SliceDurations(stats.Custom, limit));
    }

    public async Task<AsyncFactionsSummaryStatsDto> GetFactionsAsync(CancellationToken cancellationToken)
    {
        return (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Factions), "all", null, cancellationToken)).Payload.Factions;
    }

    public async Task<AsyncOpponentsSummaryStatsDto> GetOpponentsAsync(int limit, CancellationToken cancellationToken)
    {
        ValidateLimit(limit);
        var stats = (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.Opponents), "all", limit, cancellationToken)).Payload.Opponents;
        return new AsyncOpponentsSummaryStatsDto(
            SliceOpponents(stats.All, limit), SliceOpponents(stats.Tigl, limit), SliceOpponents(stats.Custom, limit));
    }

    public async Task<AsyncHistorySummaryStatsDto> GetHistoryAsync(CancellationToken cancellationToken)
    {
        return (await GetSnapshotAsync(nameof(AsyncStatisticsSnapshotDto.History), "all", null, cancellationToken)).Payload.History;
    }

    private void ValidateLimit(int limit)
    {
        if (limit is not (20 or 50 or 100 or CanonicalLimit))
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be 20, 50, 100, or 200.");
        }
    }

    private async Task<CachedSnapshot> GetSnapshotAsync(string category, string filter, int? limit, CancellationToken cancellationToken)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        if (_memoryCache.TryGetValue(MemoryCacheKey, out CachedSnapshot? cachedSnapshot)
            && cachedSnapshot is not null)
        {
            _logger.LogDebug("Async statistics snapshot cache hit; CacheLayer={CacheLayer}, SnapshotVersion={SnapshotVersion}, Category={Category}, Filter={Filter}, Limit={Limit}, DurationMilliseconds={DurationMilliseconds}", "memory", cachedSnapshot.Version, category, filter, limit, stopwatch.ElapsedMilliseconds);
            return cachedSnapshot;
        }

        await CachePopulationLock.WaitAsync(cancellationToken);
        try
        {
            if (_memoryCache.TryGetValue(MemoryCacheKey, out cachedSnapshot)
                && cachedSnapshot is not null)
            {
                _logger.LogDebug("Async statistics snapshot cache hit; CacheLayer={CacheLayer}, SnapshotVersion={SnapshotVersion}, Category={Category}, Filter={Filter}, Limit={Limit}, DurationMilliseconds={DurationMilliseconds}", "memory-after-lock", cachedSnapshot.Version, category, filter, limit, stopwatch.ElapsedMilliseconds);
                return cachedSnapshot;
            }

            var snapshot = await _repository.GetPublishedAsync(cancellationToken)
                ?? throw new InvalidOperationException("Async statistics snapshot is not available.");

            var distributedKey = $"{DistributedCacheKeyPrefix}{snapshot.SnapshotVersion}";
            var payload = await _distributedCache.GetStringAsync(distributedKey, cancellationToken);
            if (payload is null)
            {
                _logger.LogDebug("Async statistics snapshot cache miss; CacheLayer={CacheLayer}, SnapshotVersion={SnapshotVersion}, Category={Category}, Filter={Filter}, Limit={Limit}, DurationMilliseconds={DurationMilliseconds}", "distributed", snapshot.SnapshotVersion, category, filter, limit, stopwatch.ElapsedMilliseconds);
                payload = snapshot.Payload;
                await _distributedCache.SetStringAsync(
                    distributedKey,
                    payload,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheDuration },
                    cancellationToken);
            }
            else
            {
                _logger.LogDebug("Async statistics snapshot cache hit; CacheLayer={CacheLayer}, SnapshotVersion={SnapshotVersion}, Category={Category}, Filter={Filter}, Limit={Limit}, DurationMilliseconds={DurationMilliseconds}", "distributed", snapshot.SnapshotVersion, category, filter, limit, stopwatch.ElapsedMilliseconds);
            }

            var materializedSnapshot = JsonSerializer.Deserialize<AsyncStatisticsSnapshotDto>(payload)
                ?? throw new InvalidOperationException("Async statistics snapshot payload is invalid.");

            var result = new CachedSnapshot(snapshot.SnapshotVersion, materializedSnapshot);
            _memoryCache.Set(MemoryCacheKey, result, CacheDuration);
            _logger.LogDebug("Async statistics snapshot materialized; SnapshotVersion={SnapshotVersion}, Category={Category}, Filter={Filter}, Limit={Limit}, DurationMilliseconds={DurationMilliseconds}", snapshot.SnapshotVersion, category, filter, limit, stopwatch.ElapsedMilliseconds);
            return result;
        }
        finally
        {
            CachePopulationLock.Release();
        }
    }

    private AsyncWinsStatsDto SliceWins(AsyncWinsStatsDto stats, int limit) =>
        new(stats.AsyncWinsPlayers.Take(limit).ToList(), stats.AsyncWinsPercentagePlayers.Take(limit).ToList(), stats.AsyncWinsDeviationPlayers.Take(limit).ToList());

    private AsyncVpStatsDto SliceVp(AsyncVpStatsDto stats, int limit) =>
        new(stats.VpPercentagesPlayers.Take(limit).ToList(), stats.MostVpPlayers.Take(limit).ToList());

    private AsyncEliminationsStatsDto SliceEliminations(AsyncEliminationsStatsDto stats, int limit) =>
        new(stats.MostEliminationsPlayers.Take(limit).ToList(), stats.MostEliminationsPercentagePlayers.Take(limit).ToList());

    private AsyncTurnsStatsDto SliceTurns(AsyncTurnsStatsDto stats, int limit) =>
        new(stats.AsyncFastestPlayers.Take(limit).ToList(), stats.AsyncMostTurnsPlayers.Take(limit).ToList());

    private AsyncCombatStatsDto SliceCombat(AsyncCombatStatsDto stats, int limit) =>
        new(stats.TotalHitsPlayers.Take(limit).ToList(), stats.MaxHitsPerGamePlayers.Take(limit).ToList(), stats.MaxAverageHitsPerGamePlayers.Take(limit).ToList(), stats.BestHitsDeviationPlayers.Take(limit).ToList(), stats.WorstHitsDeviationPlayers.Take(limit).ToList());

    private AsyncDurationsStatsDto SliceDurations(AsyncDurationsStatsDto stats, int limit) =>
        new(stats.LongestGames.Take(limit).ToList(), stats.FastestGames.Take(limit).ToList());

    private AsyncOpponentsStatsDto SliceOpponents(AsyncOpponentsStatsDto stats, int limit) =>
        new(stats.PlayersWithMostOpponents.Take(limit).ToList());

    private sealed record CachedSnapshot(long Version, AsyncStatisticsSnapshotDto Payload);
}
