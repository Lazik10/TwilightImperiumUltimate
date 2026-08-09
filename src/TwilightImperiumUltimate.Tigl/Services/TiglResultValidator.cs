using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglResultValidator : ITiglResultValidator
{
    public Task<Result> ValidateResult(IGameReport gameReport)
    {
        // Validate that two specific galactic events did not occur in the same game
        if (gameReport.Events.Contains("Total War") && gameReport.Events.Contains("Rapid Mobilization"))
        {
            return Task.FromResult(Result.Fail($"Game {gameReport.GameId} contains both Total War and Rapid Mobilization events, which is not allowed. Please report game manually or contact @lazik2110"));
        }

        // Validate that only one player reached the final score
        var playersAtFinalScore = gameReport.PlayerResults.Count(x => x.Score >= gameReport.Score);

        var result = playersAtFinalScore switch
        {
            0 => Result.Fail($"No player reached required points for game: {gameReport.GameId}. Please report game manually or contact @lazik2110"),
            1 => Result.Ok(),
            _ => Result.Fail($"More than one player reached the final score of {gameReport.Score} for game: {gameReport.GameId}. Please report game manually or contact @lazik2110"),
        };

        return Task.FromResult(result);
    }
}
