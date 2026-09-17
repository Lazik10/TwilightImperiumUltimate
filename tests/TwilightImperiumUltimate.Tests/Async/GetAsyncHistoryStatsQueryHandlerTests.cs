using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.Async;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class GetAsyncHistoryStatsQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenSnapshotContainsEndedHistory_ShouldReturnSnapshotData()
    {
        var snapshotReader = new Mock<IAsyncStatisticsSnapshotReader>();

        var snapshot = CreateSummary(
            endedAll: [new AsyncGamesHistoryDto(2026, 9, 536, 0, 536)],
            endedTigl: [new AsyncGamesHistoryDto(2026, 9, 137, 0, 137)],
            endedCustom: [new AsyncGamesHistoryDto(2026, 9, 399, 0, 399)]);

        snapshotReader
            .Setup(x => x.GetHistoryAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(snapshot);

        var handler = new GetAsyncHistoryStatsQueryHandler(snapshotReader.Object);

        var result = await handler.Handle(new GetAsyncHistoryStatsQuery(), CancellationToken.None);

        result.Should().BeSameAs(snapshot);
        snapshotReader.Verify(x => x.GetHistoryAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSnapshotMissesEndedHistory_ShouldReturnSnapshotData()
    {
        var snapshotReader = new Mock<IAsyncStatisticsSnapshotReader>();

        var legacySnapshot = CreateSummary(
            endedAll: [],
            endedTigl: [],
            endedCustom: []);

        snapshotReader
            .Setup(x => x.GetHistoryAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(legacySnapshot);

        var handler = new GetAsyncHistoryStatsQueryHandler(snapshotReader.Object);

        var result = await handler.Handle(new GetAsyncHistoryStatsQuery(), CancellationToken.None);

        result.Should().BeSameAs(legacySnapshot);
        snapshotReader.Verify(x => x.GetHistoryAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static AsyncHistorySummaryStatsDto CreateSummary(
        IReadOnlyCollection<AsyncGamesHistoryDto> endedAll,
        IReadOnlyCollection<AsyncGamesHistoryDto> endedTigl,
        IReadOnlyCollection<AsyncGamesHistoryDto> endedCustom)
    {
        var games = new List<AsyncGamesHistoryDto>
        {
            new(2026, 9, 1, 1, 0),
        };

        var players = new List<AsyncPlayersHistoryDto>
        {
            new(2026, 9, 1, 1),
        };

        return new AsyncHistorySummaryStatsDto(
            new AsyncHistoryStatsDto(games, endedAll, players),
            new AsyncHistoryStatsDto(games, endedTigl, players),
            new AsyncHistoryStatsDto(games, endedCustom, players));
    }
}
