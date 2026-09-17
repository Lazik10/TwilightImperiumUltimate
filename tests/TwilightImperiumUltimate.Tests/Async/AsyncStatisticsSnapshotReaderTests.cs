using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncStatisticsSnapshotReaderTests
{
    [Fact]
    public async Task GetGamesAsync_WhenCanonicalSnapshotContainsMoreRows_ShouldSliceAllFiltersToRequestedLimit()
    {
        var payload = new AsyncStatisticsSnapshotDto
        {
            Games = new AsyncGamesSummaryStatsDto(
                CreateGamesStats(),
                CreateGamesStats(),
                CreateGamesStats()),
        };
        var repository = new Mock<IAsyncStatisticsSnapshotRepository>();
        repository
            .Setup(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AsyncStatisticsSnapshot { IsPublished = true, SourceDataVersion = AsyncStatisticsSnapshotSchema.Version, Payload = JsonSerializer.Serialize(payload) });
        var builder = new Mock<IAsyncStatisticsSnapshotBuilder>(MockBehavior.Strict);
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, builder.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        var result = await reader.GetGamesAsync(20, CancellationToken.None);

        result.All.MostGames.Should().HaveCount(20);
        result.Tigl.MostActiveGames.Should().HaveCount(20);
        result.Custom.MostGames.Should().HaveCount(20);
        repository.Verify(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetGeneralAsync_WhenNoSnapshotIsPublished_ShouldFailExplicitly()
    {
        var repository = new Mock<IAsyncStatisticsSnapshotRepository>();
        repository
            .Setup(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((AsyncStatisticsSnapshot?)null);
        var builder = new Mock<IAsyncStatisticsSnapshotBuilder>(MockBehavior.Strict);
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, builder.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        var action = () => reader.GetGeneralAsync(CancellationToken.None);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Async statistics snapshot is not available.");
    }

    [Fact]
    public async Task GetGeneralAsync_WhenSnapshotIsCached_ShouldReuseMemoryCache()
    {
        var repository = new Mock<IAsyncStatisticsSnapshotRepository>();
        repository
            .Setup(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateSnapshot(1));
        var builder = new Mock<IAsyncStatisticsSnapshotBuilder>(MockBehavior.Strict);
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, builder.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        await reader.GetGeneralAsync(CancellationToken.None);
        await reader.GetGeneralAsync(CancellationToken.None);

        repository.Verify(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetGeneralAsync_WhenInvalidated_ShouldLoadNewSnapshotVersion()
    {
        var repository = new Mock<IAsyncStatisticsSnapshotRepository>();
        repository
            .SetupSequence(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateSnapshot(1))
            .ReturnsAsync(CreateSnapshot(2));
        var builder = new Mock<IAsyncStatisticsSnapshotBuilder>(MockBehavior.Strict);
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, builder.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        await reader.GetGeneralAsync(CancellationToken.None);
        reader.Invalidate();
        await reader.GetGeneralAsync(CancellationToken.None);

        repository.Verify(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetGeneralAsync_WhenRequestsArriveConcurrently_ShouldPopulateCacheOnce()
    {
        var repository = new Mock<IAsyncStatisticsSnapshotRepository>();
        var loadStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLoad = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        repository
            .Setup(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                loadStarted.SetResult();
                await releaseLoad.Task;
                return CreateSnapshot(1);
            });
        var builder = new Mock<IAsyncStatisticsSnapshotBuilder>(MockBehavior.Strict);
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, builder.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        var firstRequest = reader.GetGeneralAsync(CancellationToken.None);
        await loadStarted.Task;
        var secondRequest = reader.GetGeneralAsync(CancellationToken.None);
        releaseLoad.SetResult();
        await Task.WhenAll(firstRequest, secondRequest);

        repository.Verify(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetGeneralAsync_WhenSnapshotSchemaIsIncompatible_ShouldRebuildAndPublishCompatibleSnapshot()
    {
        var incompatibleSnapshot = CreateSnapshot(1, sourceDataVersion: null);
        var rebuiltPayload = new AsyncStatisticsSnapshotDto
        {
            General = new AsyncGeneralSummaryStatsDto(
                new AsyncGeneralStatsDto(games: 99, active: 0, cancelled: 0, finished: 0, eliminations: 0, players: 0, activePlayers: 0, inactivePlayers: 0, inactiveLessThanThreeMonths: 0, inactiveMoreThanThreeMonths: 0, distributionByVp: [], distributionByPlayerTimers: [], distributionByPlayerCount: [], distributionByAverageTurnEnd: []),
                new AsyncGeneralStatsDto(),
                new AsyncGeneralStatsDto()),
        };

        var repository = new Mock<IAsyncStatisticsSnapshotRepository>();
        repository
            .Setup(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(incompatibleSnapshot);
        repository
            .Setup(x => x.PublishAsync(It.IsAny<DateTime>(), AsyncStatisticsSnapshotSchema.Version, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AsyncStatisticsSnapshot
            {
                SnapshotVersion = 2,
                IsPublished = true,
                SourceDataVersion = AsyncStatisticsSnapshotSchema.Version,
                Payload = JsonSerializer.Serialize(rebuiltPayload),
            });

        var builder = new Mock<IAsyncStatisticsSnapshotBuilder>();
        builder
            .Setup(x => x.BuildAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(rebuiltPayload);

        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, builder.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        var result = await reader.GetGeneralAsync(CancellationToken.None);

        result.All.Games.Should().Be(99);
        builder.Verify(x => x.BuildAsync(It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.PublishAsync(It.IsAny<DateTime>(), AsyncStatisticsSnapshotSchema.Version, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetGeneralAsync_WhenSnapshotPayloadIsInvalid_ShouldRebuildAndPublishCompatibleSnapshot()
    {
        var invalidPayload = new AsyncStatisticsSnapshotDto
        {
            History = new AsyncHistorySummaryStatsDto(
                new AsyncHistoryStatsDto(
                    gamesHistory: [new AsyncGamesHistoryDto(2026, 9, 2, 2, 0)],
                    gamesEndedHistory: [new AsyncGamesHistoryDto(2026, 9, 2, 0, 0)],
                    playersHistory: [new AsyncPlayersHistoryDto(2026, 9, 1, 1)]),
                new AsyncHistoryStatsDto(),
                new AsyncHistoryStatsDto()),
        };

        var rebuiltPayload = new AsyncStatisticsSnapshotDto
        {
            General = new AsyncGeneralSummaryStatsDto(
                new AsyncGeneralStatsDto(games: 123, active: 0, cancelled: 0, finished: 0, eliminations: 0, players: 0, activePlayers: 0, inactivePlayers: 0, inactiveLessThanThreeMonths: 0, inactiveMoreThanThreeMonths: 0, distributionByVp: [], distributionByPlayerTimers: [], distributionByPlayerCount: [], distributionByAverageTurnEnd: []),
                new AsyncGeneralStatsDto(),
                new AsyncGeneralStatsDto()),
        };

        var repository = new Mock<IAsyncStatisticsSnapshotRepository>();
        repository
            .Setup(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AsyncStatisticsSnapshot
            {
                IsPublished = true,
                SnapshotVersion = 7,
                SourceDataVersion = AsyncStatisticsSnapshotSchema.Version,
                Payload = JsonSerializer.Serialize(invalidPayload),
            });
        repository
            .Setup(x => x.PublishAsync(It.IsAny<DateTime>(), AsyncStatisticsSnapshotSchema.Version, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AsyncStatisticsSnapshot
            {
                SnapshotVersion = 8,
                IsPublished = true,
                SourceDataVersion = AsyncStatisticsSnapshotSchema.Version,
                Payload = JsonSerializer.Serialize(rebuiltPayload),
            });

        var builder = new Mock<IAsyncStatisticsSnapshotBuilder>();
        builder
            .Setup(x => x.BuildAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(rebuiltPayload);

        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, builder.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        var result = await reader.GetGeneralAsync(CancellationToken.None);

        result.All.Games.Should().Be(123);
        builder.Verify(x => x.BuildAsync(It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.PublishAsync(It.IsAny<DateTime>(), AsyncStatisticsSnapshotSchema.Version, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static AsyncStatisticsSnapshot CreateSnapshot(long version, string? sourceDataVersion = AsyncStatisticsSnapshotSchema.Version)
    {
        return new AsyncStatisticsSnapshot
        {
            IsPublished = true,
            SnapshotVersion = version,
            SourceDataVersion = sourceDataVersion,
            Payload = JsonSerializer.Serialize(new AsyncStatisticsSnapshotDto()),
        };
    }

    private static AsyncGamesStatsDto CreateGamesStats()
    {
        var players = Enumerable.Range(1, 200)
            .Select(index => new AsyncGamesPlayerDto(index, $"Player {index}", index, 0))
            .ToList();

        return new AsyncGamesStatsDto(players, players);
    }
}
