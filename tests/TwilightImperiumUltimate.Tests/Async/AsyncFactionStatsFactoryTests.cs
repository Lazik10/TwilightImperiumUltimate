using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncFactionStatsFactoryTests
{
    [Fact]
    public async Task CreateAsyncFactionStatsSummary_WhenFrankenAndHomebrewPlayersArePresent_ShouldCountPlayerRowsAndWins()
    {
        var repository = new Mock<IAsyncStatsRepository>();
        repository
            .Setup(x => x.GetAsyncStatisticsGameProjections(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                CreateProjection(1, true, AsyncFactionName.Franken, false),
                CreateProjection(1, true, AsyncFactionName.Franken, true),
                CreateProjection(2, false, AsyncFactionName.Homebrew, false),
                CreateProjection(2, false, AsyncFactionName.Homebrew, true),
                CreateProjection(2, false, AsyncFactionName.Homebrew, false),
            ]);
        var factory = new AsyncFactionStatsFactory(repository.Object);

        var result = await factory.CreateAsyncFactionStatsSummary(CancellationToken.None);

        var frankenStats = GetFactionStats(result.All, AsyncFactionName.Franken).All;
        var homebrewStats = GetFactionStats(result.All, AsyncFactionName.Homebrew).All;

        frankenStats.Games.Should().Be(2);
        frankenStats.Wins.Should().Be(1);
        frankenStats.WinsPercentage.Should().Be(50);
        homebrewStats.Games.Should().Be(3);
        homebrewStats.Wins.Should().Be(1);
        homebrewStats.WinsPercentage.Should().BeApproximately(33.33f, 0.01f);
    }

    private static AsyncStatisticsGameProjection CreateProjection(
        int gameStatsId,
        bool frankenGame,
        AsyncFactionName factionName,
        bool winner = false)
    {
        return new(
            gameStatsId,
            $"game-{gameStatsId}",
            string.Empty,
            0,
            0,
            1,
            true,
            6,
            0,
            10,
            false,
            false,
            frankenGame,
            1,
            factionName,
            10,
            0,
            0,
            0,
            0,
            false,
            winner);
    }

    private static AsyncFactionsStatsDto GetFactionStats(
        IReadOnlyCollection<AsyncFactionsStatsDto> factionStats,
        AsyncFactionName factionName)
    {
        return factionStats.Single(x => x.FactionName == factionName);
    }
}
