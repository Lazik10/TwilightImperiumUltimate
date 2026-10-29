using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglResultValidator : ITiglResultValidator
{
    public Task<Result> ValidateResult(IGameReport gameReport)
    {
        ArgumentNullException.ThrowIfNull(gameReport);

        // Validate that two specific galactic events did not occur in the same game
        if (gameReport.Events.Contains("Total War") && gameReport.Events.Contains("Rapid Mobilization"))
        {
            return Task.FromResult(Result.Fail($"Game {gameReport.GameId} contains both Total War and Rapid Mobilization events, which is not allowed. Please report game manually or contact @lazik2110"));
        }

        var winners = gameReport.PlayerResults.Where(player => player.IsWinner).ToList();
        if (winners.Count != 1)
        {
            return Task.FromResult(Result.Fail($"Game {gameReport.GameId} must have exactly one winner."));
        }

        if (winners[0].Score < gameReport.Score)
        {
            return Task.FromResult(Result.Fail($"The winner of game {gameReport.GameId} did not reach the final score of {gameReport.Score}."));
        }

        var playersAtFinalScore = gameReport.PlayerResults.Count(x => x.Score >= gameReport.Score);
        var isCivilizedSocietyTie = gameReport.League == TiglLeague.Fractured
            && gameReport.Events.Contains("Civilized Society", StringComparer.OrdinalIgnoreCase)
            && playersAtFinalScore is >= 2 and <= 8;

        var result = playersAtFinalScore switch
        {
            0 => Result.Fail($"No player reached required points for game: {gameReport.GameId}. Please report game manually or contact @lazik2110"),
            1 => Result.Ok(),
            _ when isCivilizedSocietyTie => Result.Ok(),
            _ => Result.Fail($"More than one player reached the final score of {gameReport.Score} for game: {gameReport.GameId}. Please report game manually or contact @lazik2110"),
        };

        return Task.FromResult(result);
    }
}
