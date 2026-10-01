using System.Globalization;
using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncHistoryStatsFactoryTests
{
    [Fact]
    public async Task CreateAsyncHistoryStatsSummary_WhenGamesHaveEndedDates_ShouldCountFinishedGamesByMonth()
    {
        var repository = new Mock<IAsyncStatsRepository>();
        repository
            .Setup(x => x.GetAsyncStatisticsGameProjections(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGameRows());

        var factory = new AsyncHistoryStatsFactory(repository.Object);

        var result = await factory.CreateAsyncHistoryStatsSummary(CancellationToken.None);

        result.All.GamesEndedHistory.Should().ContainSingle(x => x.Year == 2025 && x.Month == 1 && x.Ended == 2);
        result.All.GamesEndedHistory.Should().ContainSingle(x => x.Year == 2025 && x.Month == 2 && x.Ended == 1);
        result.All.GamesEndedHistory.Should().OnlyContain(x => x.Ended >= 0);
    }

    [Fact]
    public async Task CreateAsyncHistoryStatsSummary_WhenGameHasWinnerButNoEndedTimestamp_ShouldStillCountItAsEnded()
    {
        var repository = new Mock<IAsyncStatsRepository>();
        repository
            .Setup(x => x.GetAsyncStatisticsGameProjections(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new(1, "game-1", string.Empty, 1735689600, 1735689600, null, true, 6, 0, true, false, false, 1001, null, 10, 5, 600, 0, 0, null, null),
                new(1, "game-1", string.Empty, 1735689600, 1735689600, null, true, 6, 0, true, false, false, 1002, null, 15, 7, 600, 0, 0, null, null),
            ]);

        var factory = new AsyncHistoryStatsFactory(repository.Object);

        var result = await factory.CreateAsyncHistoryStatsSummary(CancellationToken.None);

        result.All.GamesEndedHistory.Should().ContainSingle(x => x.Year == 2025 && x.Month == 1 && x.Ended == 1);
    }

    private static List<AsyncStatisticsGameProjection> CreateGameRows()
    {
        var januarySetup = DateTimeOffset.Parse("2025-01-03T00:00:00Z", CultureInfo.InvariantCulture).ToUnixTimeSeconds();
        var januaryEnded = DateTimeOffset.Parse("2025-01-12T00:00:00Z", CultureInfo.InvariantCulture).ToUnixTimeSeconds();
        var februarySetup = DateTimeOffset.Parse("2025-02-01T00:00:00Z", CultureInfo.InvariantCulture).ToUnixTimeSeconds();
        var februaryEnded = DateTimeOffset.Parse("2025-02-14T00:00:00Z", CultureInfo.InvariantCulture).ToUnixTimeSeconds();

        return
        [
            new(1, "game-1", string.Empty, januarySetup, januarySetup, januaryEnded, true, 6, 0, true, false, false, 1001, null, 10, 5, 600, 0, 0, null, null),
            new(1, "game-1", string.Empty, januarySetup, januarySetup, januaryEnded, true, 6, 0, true, false, false, 1002, null, 15, 7, 600, 0, 0, null, null),
            new(2, "game-2", string.Empty, januarySetup, januarySetup, januaryEnded, true, 6, 0, false, false, false, 1003, null, 20, 8, 600, 0, 0, null, null),
            new(2, "game-2", string.Empty, januarySetup, januarySetup, januaryEnded, true, 6, 0, false, false, false, 1004, null, 5, 4, 600, 0, 0, null, null),
            new(3, "game-3", string.Empty, februarySetup, februarySetup, februaryEnded, true, 6, 0, true, false, false, 1005, null, 18, 6, 600, 0, 0, null, null),
        ];
    }
}
