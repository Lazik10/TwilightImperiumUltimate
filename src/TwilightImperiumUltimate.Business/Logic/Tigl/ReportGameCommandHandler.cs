using Microsoft.Extensions.Logging;
using System.Text;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Tigl.AsyncRating;
using TwilightImperiumUltimate.Tigl.Glicko2Rating;
using TwilightImperiumUltimate.Tigl.Services;
using TwilightImperiumUltimate.Tigl.TrueSkillRating;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class ReportGameCommandHandler(
    ITiglMatchUsersValidator tiglMatchUserValidator,
    ITiglMatchInserter matchInserter,
    IAsyncRatingProcessor asyncRatingProcessor,
    IGlickoRatingProcessor glickoRatingProcessor,
    ITrueSkillRatingProcessor trueSkillRatingProcessor,
    ITiglRepository tiglRepository,
    ITiglFactionValidator tiglFactionValidator,
    ITiglResultValidator tiglResultValidator,
    ILogger<ReportGameCommandHandler> logger)
    : IRequestHandler<ReportGameCommand, GameReportResult>
{
    private const int RequiredUserCount = 6;
    private readonly ILogger<ReportGameCommandHandler> _logger = logger;

    public async Task<GameReportResult> Handle(ReportGameCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var evaluationEnabled = await tiglRepository.GetTiglParameter(TiglParameterName.EvaluateGames, cancellationToken);
        if (evaluationEnabled is null || !evaluationEnabled.Enabled)
        {
            _logger.LogWarning("Game evaluation is disabled, skipping game report processing for GameID: {GameId}", request.GameReport.GameId);
            return new GameReportResult { Success = false, ErrorTitle = "Game evaluation was disabled, please contact @lazik2110 for help." };
        }

        var newGame = await tiglRepository.GetMatchReport(request.GameReport.GameId, cancellationToken);
        if (newGame is not null)
        {
            _logger.LogError("Game report for GameID: {GameId} already exists in the database.", request.GameReport.GameId);
            return new GameReportResult(false, "This game was already reported", $"A game report for game with ID {request.GameReport.GameId} already exists in the database.");
        }

        var gameReport = request.GameReport;

        if (gameReport.PlayerResults.Count != RequiredUserCount)
        {
            _logger.LogError("Invalid player count for game report {GameReportId}", request.GameReport.GameId);
            return new GameReportResult(false, "Invalid Player Count", "Not enough player results in the game report.");
        }

        var factionValidationResult = await tiglFactionValidator.AllTiglFactionsAreValid(gameReport);
        if (factionValidationResult.IsFailed)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var error in factionValidationResult.Errors)
            {
                stringBuilder.AppendLine(error.Message);
            }

            return new GameReportResult(false, "Invalid Factions", stringBuilder.ToString());
        }

        // Validate that all users exist in the database
        var result = await tiglMatchUserValidator.AllTiglUsersExists(gameReport, cancellationToken);
        if (!result)
        {
            var registerNewUsersResult = await tiglMatchUserValidator.RegisterNewTiglUsers(gameReport, cancellationToken);
            if (registerNewUsersResult.IsFailed)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in registerNewUsersResult.Errors)
                {
                    stringBuilder.AppendLine(error.Message);
                }

                return new GameReportResult(false, "Failed to register player(s)", stringBuilder.ToString());
            }
        }

        // Validate that all player results are valid
        var scoreValidationResult = await tiglResultValidator.ValidateResult(gameReport);
        if (!scoreValidationResult)
            return new GameReportResult(false, "Invalid Player Results", $"No player reached required points for game: {gameReport.GameId}");

        var insertResult = await matchInserter.InsertGameReport(gameReport, cancellationToken);
        if (insertResult.IsFailed)
        {
            foreach (var error in insertResult.Errors)
            {
                _logger.LogError("Insert game report error: {InsertError}", error.Message);
            }

            return new GameReportResult { Success = false, ErrorTitle = $"Insertion of results for game {gameReport.GameId} failed", };
        }
        else
        {
            var gameId = insertResult.Value.Id;
            await asyncRatingProcessor.ProcessGameReport(gameReport, gameId, cancellationToken);
            await glickoRatingProcessor.ProcessGameReport(gameReport, gameId, cancellationToken);
            await trueSkillRatingProcessor.ProcessGameReport(gameReport, gameId, cancellationToken);
        }

        var matchUpdateResult = await tiglRepository.UpdateMatch(insertResult.Value.Id, cancellationToken);
        if (matchUpdateResult.IsFailed)
        {
            _logger.LogError("Failed to update match for GameID: {GameId}", gameReport.GameId);
            return new GameReportResult(false, "Failed to mark game as processed", "There was an error when marking game as processed in the database!");
        }

        return new GameReportResult() { Success = true };
    }
}
