using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
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
            .ReturnsAsync(new AsyncStatisticsSnapshot { IsPublished = true, Payload = JsonSerializer.Serialize(payload) });
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

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
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

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
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

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
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

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
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var reader = new AsyncStatisticsSnapshotReader(repository.Object, cache, distributedCache, NullLogger<AsyncStatisticsSnapshotReader>.Instance);

        var firstRequest = reader.GetGeneralAsync(CancellationToken.None);
        await loadStarted.Task;
        var secondRequest = reader.GetGeneralAsync(CancellationToken.None);
        releaseLoad.SetResult();
        await Task.WhenAll(firstRequest, secondRequest);

        repository.Verify(x => x.GetPublishedAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static AsyncStatisticsSnapshot CreateSnapshot(long version)
    {
        return new AsyncStatisticsSnapshot
        {
            IsPublished = true,
            SnapshotVersion = version,
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
