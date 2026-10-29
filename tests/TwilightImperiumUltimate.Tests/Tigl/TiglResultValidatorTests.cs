using FluentAssertions;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Tigl.Services;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public sealed class TiglResultValidatorTests
{
    [Fact]
    public async Task ValidateResult_WhenFracturedCivilizedSocietyHasTwoPlayersAtFinalScore_ShouldSucceed()
    {
        // Arrange
        var report = CreateReport(
            TiglLeague.Fractured,
            ["Civilized Society"],
            [
                new PlayerResult { Score = 10, IsWinner = true },
                new PlayerResult { Score = 10 },
            ]);
        var validator = new TiglResultValidator();

        // Act
        var result = await validator.ValidateResult(report);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateResult_WhenTwoPlayersReachFinalScoreWithoutCivilizedSociety_ShouldFail()
    {
        // Arrange
        var report = CreateReport(
            TiglLeague.Fractured,
            [],
            [
                new PlayerResult { Score = 10, IsWinner = true },
                new PlayerResult { Score = 10 },
            ]);
        var validator = new TiglResultValidator();

        // Act
        var result = await validator.ValidateResult(report);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateResult_WhenCivilizedSocietyHasMultipleWinners_ShouldFail()
    {
        // Arrange
        var report = CreateReport(
            TiglLeague.Fractured,
            ["Civilized Society"],
            [
                new PlayerResult { Score = 10, IsWinner = true },
                new PlayerResult { Score = 10, IsWinner = true },
            ]);
        var validator = new TiglResultValidator();

        // Act
        var result = await validator.ValidateResult(report);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateResult_WhenCivilizedSocietyHasThreePlayersAtFinalScore_ShouldSucceed()
    {
        // Arrange
        var report = CreateReport(
            TiglLeague.Fractured,
            ["Civilized Society"],
            [
                new PlayerResult { Score = 10, IsWinner = true },
                new PlayerResult { Score = 10 },
                new PlayerResult { Score = 10 },
            ]);
        var validator = new TiglResultValidator();

        // Act
        var result = await validator.ValidateResult(report);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateResult_WhenCivilizedSocietyHasEightPlayersAtFinalScore_ShouldSucceed()
    {
        // Arrange
        var players = Enumerable.Range(0, 8)
            .Select(index => new PlayerResult
            {
                Score = 10,
                IsWinner = index == 0,
            })
            .ToList();
        var report = CreateReport(TiglLeague.Fractured, ["Civilized Society"], players);
        var validator = new TiglResultValidator();

        // Act
        var result = await validator.ValidateResult(report);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    private static GameReport CreateReport(
        TiglLeague league,
        List<string> events,
        IReadOnlyCollection<PlayerResult> playerResults)
        => new()
        {
            GameId = "test-game",
            League = league,
            Score = 10,
            Events = events,
            PlayerResults = playerResults,
        };
}