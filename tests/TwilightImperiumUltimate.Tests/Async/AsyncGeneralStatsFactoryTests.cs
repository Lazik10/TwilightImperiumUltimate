using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncGeneralStatsFactoryTests
{
    [Fact]
    public async Task CreateAsyncGeneralStatsSummary_WhenProjectionContainsAllGameCategories_ShouldPreserveStatistics()
    {
        var repository = new Mock<IAsyncStatsRepository>();
        repository
            .Setup(x => x.GetAsyncStatisticsGameProjections(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGameRows());
        var factory = new AsyncGeneralStatsFactory(repository.Object);

        var result = await factory.CreateAsyncGeneralStatsSummary(CancellationToken.None);

        result.All.Games.Should().Be(3);
        result.All.Active.Should().Be(1);
        result.All.Cancelled.Should().Be(1);
        result.All.Finished.Should().Be(1);
        result.All.Eliminations.Should().Be(1);
        result.All.Players.Should().Be(3);
        result.All.ActivePlayers.Should().Be(2);
        result.All.InactivePlayers.Should().Be(1);
        result.All.InactiveLessThanThreeMonths.Should().Be(1);
        result.All.InactiveMoreThanThreeMonths.Should().Be(0);
        result.All.DistributionByVp.Should().ContainSingle(x => x.Vp == 10 && x.Games == 1);
        result.All.DistributionByPlayerCount.Should().ContainSingle(x => x.PlayerCount == 6 && x.Games == 1);
        result.All.DistributionByPlayerTimers.Should().Contain(x => x.Timer == 1 && x.Count == 2);
        result.Tigl.Games.Should().Be(2);
        result.Custom.Games.Should().Be(1);
        repository.Verify(x => x.GetAsyncStatisticsGameProjections(It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.GetAllAsyncGames(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static List<AsyncStatisticsGameProjection> CreateGameRows()
    {
        var now = DateTimeOffset.UtcNow;
        var oneMonthAgo = now.AddMonths(-1).ToUnixTimeSeconds();
        var fourMonthsAgo = now.AddMonths(-4).ToUnixTimeSeconds();

        return
        [
            new(1, "g1", "fun1", now.ToUnixTimeSeconds(), now.ToUnixTimeSeconds(), null, false, 6, 3, 10, true, false, false, 1001, null, null, 120, 180000000, null, null, false, null),
            new(1, "g1", "fun1", now.ToUnixTimeSeconds(), now.ToUnixTimeSeconds(), null, false, 6, 3, 10, true, false, false, 1002, null, null, 80, 180000000, null, null, false, null),
            new(2, "g2", "fun2", fourMonthsAgo, fourMonthsAgo, fourMonthsAgo, false, 9, 5, 12, false, false, false, 1001, null, null, 200, 720000000, null, null, true, null),
            new(2, "g2", "fun2", fourMonthsAgo, fourMonthsAgo, fourMonthsAgo, false, 9, 5, 12, false, false, false, 1003, null, null, 0, 0, null, null, false, null),
            new(3, "g3", "fun3", oneMonthAgo, oneMonthAgo, oneMonthAgo, true, 8, 7, 14, true, false, false, 1003, null, null, 120, 360000000, null, null, false, null),
        ];
    }
}
