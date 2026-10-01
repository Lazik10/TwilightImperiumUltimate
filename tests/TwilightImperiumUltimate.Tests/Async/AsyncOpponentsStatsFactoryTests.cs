using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncOpponentsStatsFactoryTests
{
    [Fact]
    public async Task CreateAsyncOpponentsStatsSummary_WhenPlayerHasNoOpponents_ShouldExcludePlayerBeforeLimit()
    {
        var repository = new Mock<IAsyncStatsRepository>();
        repository
            .Setup(x => x.GetAsyncStatisticsGameProjections(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGameRows());
        repository
            .Setup(x => x.GetAllAsyncPlayerProfiles(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateProfiles());
        var factory = new AsyncOpponentsStatsFactory(repository.Object);

        var result = await factory.CreateAsyncOpponentsStatsSummary(1, CancellationToken.None);

        result.All.PlayersWithMostOpponents.Should().ContainSingle(player =>
            player.Id == 2 && player.UniqueOpponents == 2);
    }

    private static List<AsyncStatisticsGameProjection> CreateGameRows() =>
    [
        CreateProjection(1, 1001),
        CreateProjection(2, 1002),
        CreateProjection(2, 1003),
        CreateProjection(3, 1002),
        CreateProjection(3, 1004),
    ];

    private static List<AsyncPlayerProfile> CreateProfiles() =>
    [
        CreateProfile(1001, 1),
        CreateProfile(1002, 2),
        CreateProfile(1003, 3),
        CreateProfile(1004, 4),
    ];

    private static AsyncPlayerProfile CreateProfile(long discordUserId, int id) => new()
    {
        Id = id,
        DiscordUserId = discordUserId,
        DiscordUserName = $"Player {id}",
        ProfileSettings = new AsyncPlayerProfileSettings(),
    };

    private static AsyncStatisticsGameProjection CreateProjection(int gameStatsId, long discordUserId) => new(
        gameStatsId,
        $"game-{gameStatsId}",
        $"Game {gameStatsId}",
        0,
        0,
        null,
        false,
        0,
        0,
        gameStatsId == 1,
        false,
        false,
        discordUserId,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null);
}
