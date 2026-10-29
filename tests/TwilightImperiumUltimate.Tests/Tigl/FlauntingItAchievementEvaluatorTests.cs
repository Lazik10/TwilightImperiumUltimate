using FluentResults;
using Moq;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Implementations;
using TwilightImperiumUltimate.Tigl.RankUp;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public sealed class FlauntingItAchievementEvaluatorTests
{
    [Fact]
    public async Task EvaluateAsync_WhenWinnerStartsAtHero_ShouldAwardAchievement()
    {
        // Arrange
        var prestigeRankService = new Mock<IPrestigeRankService>();
        var achievementRepository = new Mock<IAchievementRepository>();
        var rankRepository = CreateRankRepository(TiglRankName.Hero);
        var matchReport = CreateMatchReport(TiglRankName.Hero, TiglRankName.Hero, forcedReset: false);

        prestigeRankService
            .Setup(service => service.HasFactionPrestigeRankExcludingCurrentMatch(
                It.IsAny<int>(),
                matchReport,
                TiglFactionName.TheArborec,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var evaluator = new FlauntingItAchievementEvaluator(prestigeRankService.Object, achievementRepository.Object, rankRepository.Object);

        // Act
        await evaluator.EvaluateAsync(matchReport, AchievementName.FlauntingIt, []);

        // Assert
        achievementRepository.Verify(
            repository => repository.AwardAchievement(
                1,
                matchReport,
                AchievementName.FlauntingIt,
                It.IsAny<CancellationToken>(),
                TiglFactionName.TheArborec),
            Times.Once);
    }

    [Fact]
    public async Task EvaluateAsync_WhenHeroWinnerIsReset_ShouldAwardAchievement()
    {
        // Arrange
        var prestigeRankService = new Mock<IPrestigeRankService>();
        var achievementRepository = new Mock<IAchievementRepository>();
        var rankRepository = CreateRankRepository(TiglRankName.Unranked);
        var matchReport = CreateMatchReport(TiglRankName.Hero, TiglRankName.Hero, forcedReset: true);

        prestigeRankService
            .Setup(service => service.HasFactionPrestigeRankExcludingCurrentMatch(
                It.IsAny<int>(),
                matchReport,
                TiglFactionName.TheArborec,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var evaluator = new FlauntingItAchievementEvaluator(prestigeRankService.Object, achievementRepository.Object, rankRepository.Object);

        // Act
        await evaluator.EvaluateAsync(matchReport, AchievementName.FlauntingIt, []);

        // Assert
        achievementRepository.Verify(
            repository => repository.AwardAchievement(
                1,
                matchReport,
                AchievementName.FlauntingIt,
                It.IsAny<CancellationToken>(),
                TiglFactionName.TheArborec),
            Times.Once);
    }

    [Fact]
    public async Task EvaluateAsync_WhenWinnerRanksUpFromMinisterToAgent_ShouldNotAwardAchievement()
    {
        // Arrange
        var prestigeRankService = new Mock<IPrestigeRankService>();
        var achievementRepository = new Mock<IAchievementRepository>();
        var rankRepository = new Mock<IRankRepository>();
        var matchReport = CreateMatchReport(TiglRankName.Minister, TiglRankName.Agent, forcedReset: false);
        var evaluator = new FlauntingItAchievementEvaluator(prestigeRankService.Object, achievementRepository.Object, rankRepository.Object);

        // Act
        await evaluator.EvaluateAsync(matchReport, AchievementName.FlauntingIt, []);

        // Assert
        achievementRepository.Verify(
            repository => repository.AwardAchievement(
                It.IsAny<int>(),
                It.IsAny<MatchReport>(),
                It.IsAny<AchievementName>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<TiglFactionName>()),
            Times.Never);
    }

    [Fact]
    public async Task EvaluateAsync_WhenHeroWinnerWasResetByAnotherGame_ShouldNotAwardAchievement()
    {
        // Arrange
        var prestigeRankService = new Mock<IPrestigeRankService>();
        var achievementRepository = new Mock<IAchievementRepository>();
        var rankRepository = CreateRankRepository(TiglRankName.Unranked);
        var matchReport = CreateMatchReport(TiglRankName.Hero, TiglRankName.Hero, forcedReset: false);
        var evaluator = new FlauntingItAchievementEvaluator(prestigeRankService.Object, achievementRepository.Object, rankRepository.Object);

        // Act
        await evaluator.EvaluateAsync(matchReport, AchievementName.FlauntingIt, []);

        // Assert
        achievementRepository.Verify(
            repository => repository.AwardAchievement(
                It.IsAny<int>(),
                It.IsAny<MatchReport>(),
                It.IsAny<AchievementName>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<TiglFactionName>()),
            Times.Never);
    }

    private static Mock<IRankRepository> CreateRankRepository(TiglRankName currentRank)
    {
        var rankRepository = new Mock<IRankRepository>();

        rankRepository
            .Setup(repository => repository.GetCurrentRank(
                1,
                TiglLeague.ThundersEdge,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new TiglRank { Name = currentRank }));

        return rankRepository;
    }

    private static MatchReport CreateMatchReport(TiglRankName oldRank, TiglRankName newRank, bool forcedReset)
    {
        var matchReport = new MatchReport
        {
            League = TiglLeague.ThundersEdge,
            PlayerResults =
            [
                new PlayerResult
                {
                    TiglUserId = 1,
                    IsWinner = true,
                    Faction = TiglFactionName.TheArborec,
                },
            ],
        };

        matchReport.PlayerMatchAsyncStats!.Add(new AsyncPlayerMatchStats
        {
            Winner = true,
            Faction = TiglFactionName.TheArborec,
            OldRank = oldRank,
            NewRank = newRank,
            ForcedReset = forcedReset,
        });

        return matchReport;
    }
}
