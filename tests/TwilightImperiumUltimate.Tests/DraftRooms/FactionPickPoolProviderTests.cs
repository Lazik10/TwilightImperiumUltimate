using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Factions;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class FactionPickPoolProviderTests
{
    [Fact]
    public async Task GetPoolAsync_WhenVersionsAreProvided_ShouldUseExistingRepositoryAndReturnDistinctIdentifiers()
    {
        GameVersion[] versions = [GameVersion.BaseGame, GameVersion.ThundersEdge];
        using var cancellation = new CancellationTokenSource();
        var repository = new Mock<IFactionRepository>();
        repository.Setup(factions => factions.GetFactionsByGameVersions(versions, cancellation.Token))
            .ReturnsAsync(
            [
                new Faction { FactionName = FactionName.TheArborec },
                new Faction { FactionName = FactionName.TheBaronyOfLetnev },
                new Faction { FactionName = FactionName.TheArborec },
            ]);
        var provider = new FactionPickPoolProvider(repository.Object);

        var pool = await provider.GetPoolAsync(versions, cancellation.Token);

        pool.Should().Equal(FactionName.TheArborec, FactionName.TheBaronyOfLetnev);
        repository.Verify(factions => factions.GetFactionsByGameVersions(versions, cancellation.Token), Times.Once);
    }

    [Fact]
    public async Task GetPoolAsync_WhenRepositoryFails_ShouldPropagateFailureRatherThanReturnEmptyPool()
    {
        GameVersion[] versions = [GameVersion.BaseGame];
        var repository = new Mock<IFactionRepository>();
        repository.Setup(factions => factions.GetFactionsByGameVersions(versions, CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException("Database is unavailable."));
        var provider = new FactionPickPoolProvider(repository.Object);

        var load = () => provider.GetPoolAsync(versions, CancellationToken.None);

        await load.Should().ThrowAsync<InvalidOperationException>();
    }
}
