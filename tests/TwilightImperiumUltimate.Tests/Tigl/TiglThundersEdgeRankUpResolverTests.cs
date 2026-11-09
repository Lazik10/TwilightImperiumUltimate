using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.RankUp;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public sealed class TiglThundersEdgeRankUpResolverTests
{
    [Theory]
    [InlineData(4, 5, true)]
    [InlineData(5, 5, false)]
    [InlineData(5, 6, false)]
    [InlineData(9, 10, true)]
    [InlineData(10, 10, false)]
    [InlineData(24, 25, true)]
    [InlineData(25, 25, false)]
    [InlineData(29, 30, false)]
    public void ShouldResetRank_WhenPrestigeCountChanges_ShouldFollowMilestoneRules(
        int prestigeRankCountBeforeMatch,
        int prestigeRankCountAfterMatch,
        bool expected)
    {
        var shouldResetRank = TiglThundersEdgeRankUpResolver.ShouldResetRank(
            prestigeRankCountBeforeMatch,
            prestigeRankCountAfterMatch,
            rankUpStep: 5,
            maxGalaxyThreatRanks: 5);

        shouldResetRank.Should().Be(expected);
    }

    [Theory]
    [InlineData(9, 10, 2)]
    [InlineData(24, 25, 5)]
    public async Task ShouldResetRankAsync_WhenAllRequiredGalacticThreatRanksExist_ShouldReset(
        int prestigeRankCountBeforeMatch,
        int prestigeRankCountAfterMatch,
        int requiredGalacticThreatRank)
    {
        const int playerId = 1;
        var matchReport = new MatchReport { League = TiglLeague.ThundersEdge };
        var prestigeRankService = new Mock<IPrestigeRankService>();
        prestigeRankService
            .Setup(service => service.HasAllGalacticThreatRanksThrough(
                playerId,
                matchReport,
                requiredGalacticThreatRank,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var resolver = CreateResolver(prestigeRankService.Object);

        var shouldResetRank = await resolver.ShouldResetRankAsync(
            playerId,
            matchReport,
            prestigeRankCountBeforeMatch,
            prestigeRankCountAfterMatch,
            CancellationToken.None);

        shouldResetRank.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldResetRankAsync_WhenRequiredGalacticThreatRanksAreMissing_ShouldNotReset()
    {
        const int playerId = 1;
        var matchReport = new MatchReport { League = TiglLeague.ThundersEdge };
        var prestigeRankService = new Mock<IPrestigeRankService>();
        prestigeRankService
            .Setup(service => service.HasAllGalacticThreatRanksThrough(
                playerId,
                matchReport,
                2,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var resolver = CreateResolver(prestigeRankService.Object);

        var shouldResetRank = await resolver.ShouldResetRankAsync(
            playerId,
            matchReport,
            prestigeRankCountBeforeMatch: 9,
            prestigeRankCountAfterMatch: 10,
            cancellationToken: CancellationToken.None);

        shouldResetRank.Should().BeFalse();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task EvaluatePrestigeRanks_WhenThirtyFactionRanksAreReached_ShouldAwardPaxOnlyAfterAllThreatRanksExist(
        bool hasAllGalacticThreatRanks,
        bool shouldAwardPax)
    {
        const int playerId = 1;
        var matchReport = new MatchReport { League = TiglLeague.ThundersEdge };
        var prestigeRankService = new Mock<IPrestigeRankService>();
        prestigeRankService
            .Setup(service => service.HasFactionPrestigeRank(
                playerId,
                matchReport,
                It.IsAny<TiglFactionName>(),
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        prestigeRankService
            .Setup(service => service.GetFactionPrestigeRankCount(
                playerId,
                matchReport,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(30);
        prestigeRankService
            .Setup(service => service.HasAllGalacticThreatRanksThrough(
                playerId,
                matchReport,
                5,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(hasAllGalacticThreatRanks);
        prestigeRankService
            .Setup(service => service.AddPaxMagnificaBellumGloriosumRank(
                playerId,
                matchReport,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        var resolver = new TestRankUpResolver(prestigeRankService.Object);
        resolver.SetMatchReport(matchReport);

        await resolver.EvaluatePrestigeRanksAsync(
            playerId,
            matchReport,
            default,
            TiglLeague.ThundersEdge,
            CancellationToken.None);

        prestigeRankService.Verify(
            service => service.AddPaxMagnificaBellumGloriosumRank(
                playerId,
                matchReport,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()),
            shouldAwardPax ? Times.Once() : Times.Never());
    }

    private static TiglThundersEdgeRankUpResolver CreateResolver(IPrestigeRankService prestigeRankService)
    {
        return new TiglThundersEdgeRankUpResolver(
            Mock.Of<ITiglRepository>(),
            Mock.Of<ITiglUserRepository>(),
            Mock.Of<IRankRepository>(),
            prestigeRankService,
            new LeaderUpdateServiceStub());
    }

    private sealed class TestRankUpResolver(IPrestigeRankService prestigeRankService)
        : RankUpResolverBase(
            Mock.Of<ITiglRepository>(),
            Mock.Of<ITiglUserRepository>(),
            Mock.Of<IRankRepository>(),
            prestigeRankService,
            new LeaderUpdateServiceStub())
    {
        protected override int MaxPrestigeRanks => 30;

        protected override int MaxGalaxyThreatRanks => 5;

        public void SetMatchReport(MatchReport matchReport)
        {
            MatchReport = matchReport;
        }

        public Task<int> EvaluatePrestigeRanksAsync(
            int playerId,
            MatchReport matchReport,
            TiglFactionName faction,
            TiglLeague league,
            CancellationToken cancellationToken)
            => EvaluatePrestigeRanks(playerId, matchReport, faction, league, cancellationToken);
    }

    private sealed class LeaderUpdateServiceStub : ILeaderUpdateService
    {
        public Task<bool> UpdateLeader(
            TiglUser player,
            MatchReport matchReport,
            TiglFactionName faction,
            CancellationToken cancellationToken)
            => Task.FromResult(true);
    }
}
