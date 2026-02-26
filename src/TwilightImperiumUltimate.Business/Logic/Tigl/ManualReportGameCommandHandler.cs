using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Options;
using TwilightImperiumUltimate.Tigl.Services;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class ManualReportGameCommandHandler(
    ITiglMatchUsersValidator tiglMatchUserValidator,
    ITiglMatchInserter matchInserter,
    ITiglRepository tiglRepository,
    ITiglFactionValidator tiglFactionValidator,
    ITiglResultValidator tiglResultValidator,
    IOptions<TiglOptions> tiglOptions,
    ILogger<ReportGameCommandHandler> logger)
    : IRequestHandler<ManualReportGameCommand, GameReportResult>
{
    private const int MinimumRequiredPlayerCount = 6;
    private readonly ILogger<ReportGameCommandHandler> _logger = logger;
    private readonly TiglOptions _tiglOptions = tiglOptions.Value;

    public async Task<GameReportResult> Handle(ManualReportGameCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var gameReport = request.GameReport;

        var allowOnlyGamesFromThundersEdgeEra = await tiglRepository.GetTiglParameter(TiglParameterName.OnlyThundersEdgeEra, cancellationToken);
        if (allowOnlyGamesFromThundersEdgeEra is not null
            && allowOnlyGamesFromThundersEdgeEra.Enabled
            && _tiglOptions.ThundersEdgeCutoffTimestamp > 0
            && gameReport.StartTimestamp < _tiglOptions.ThundersEdgeCutoffTimestamp)
        {
            _logger.LogWarning("Game {GameId} rejected due to start timestamp {Start} before cutoff {Cutoff}", gameReport.GameId, gameReport.StartTimestamp, _tiglOptions.ThundersEdgeCutoffTimestamp);
            return new GameReportResult(false, "Game too old", "This game started before the allowed cutoff date of 1st December 2025 and cannot be reported.");
        }

        var onlyImportEnabled = await tiglRepository.GetTiglParameter(TiglParameterName.OnlyImportReports, cancellationToken);
        if (onlyImportEnabled is not null && onlyImportEnabled.Enabled && gameReport.Source != ResultSource.Test)
        {
            return new GameReportResult(false, "Only Import Reports Enabled", "The system is currently set that only imported reports can be evaluated. Please contact @lazik2110 for help.");
        }
        else
        {
            gameReport.Source = ResultSource.Async;
        }

        var enabledEvaluationValidation = await IsEvaluationDisabled(gameReport.GameId, cancellationToken);
        if (!enabledEvaluationValidation.NextValidation)
            return enabledEvaluationValidation.Result;

        var newGameValidation = await IsNewGame(gameReport.GameId, cancellationToken);
        if (!newGameValidation.NextValidation)
            return newGameValidation.Result;

        var playerCountValidation = IsValidPlayerCount(gameReport);
        if (!playerCountValidation.NextValidation)
            return playerCountValidation.Result;

        var factionValidationResult = await tiglFactionValidator.AllTiglFactionsAreValid(gameReport);
        if (factionValidationResult.IsFailed)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var error in factionValidationResult.Errors)
            {
                stringBuilder.AppendLine(error.Message);
            }

            return new GameReportResult(false, $"Invalid Factions for game {gameReport.GameId}", stringBuilder.ToString());
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

                return new GameReportResult(false, $"Failed to register player(s) for game {gameReport.GameId}", stringBuilder.ToString());
            }
        }

        // Validate that all player results are valid
        var scoreValidationResult = await tiglResultValidator.ValidateResult(gameReport);
        if (!scoreValidationResult)
            return new GameReportResult(false, "Invalid Player Results", $"No player reached required points for game: {gameReport.GameId}. Please report game manually or contact @lazik2110");

        var insertResult = await matchInserter.InsertGameReport(gameReport, cancellationToken);
        if (insertResult.IsFailed)
        {
            foreach (var error in insertResult.Errors)
            {
                _logger.LogError("Insert game report error: {InsertError}", error.Message);
            }

            return new GameReportResult { Success = false, ErrorTitle = $"Insertion of results for game {gameReport.GameId} failed", };
        }

        return new GameReportResult() { Success = true };
    }

    private (bool NextValidation, GameReportResult Result) IsValidPlayerCount(GameReport gameReport)
    {
        var playerCountEqualsResultsCount = gameReport.PlayerCount == gameReport.PlayerResults.Count;
        var isValidStandardPlayerCount = (gameReport.League == TiglLeague.ThundersEdge || gameReport.League == TiglLeague.ProphecyOfKings) && gameReport.PlayerCount == MinimumRequiredPlayerCount && playerCountEqualsResultsCount;
        var isValidFracturedPlayerCount = gameReport.League == TiglLeague.Fractured && gameReport.PlayerCount >= MinimumRequiredPlayerCount && playerCountEqualsResultsCount;

        if (isValidStandardPlayerCount || isValidFracturedPlayerCount)
            return (true, new GameReportResult());

        _logger.LogError("Invalid player count for game report {GameReportId}", gameReport.GameId);
        return (false, new GameReportResult(false, "Invalid Player Count", "Not enough player results in the game report. If one or more player(s) got eliminated please report this game manually!"));
    }

    private async Task<(bool NextValidation, GameReportResult Result)> IsEvaluationDisabled(string gameId, CancellationToken cancellationToken)
    {
        var evaluationEnabled = await tiglRepository.GetTiglParameter(TiglParameterName.EvaluateGames, cancellationToken);
        if (evaluationEnabled is null || !evaluationEnabled.Enabled)
        {
            _logger.LogWarning("Game evaluation is disabled, skipping game report processing for GameID: {GameId}", gameId);
            return (false, new GameReportResult(false, "Game evaluation disabled", "Game evaluation is currently disabled, please contact @lazik2110 for help."));
        }

        return (true, new GameReportResult());
    }

    private async Task<(bool NextValidation, GameReportResult Result)> IsNewGame(string gameId, CancellationToken cancellationToken)
    {
        var game = await tiglRepository.GetMatchReportByGameId(gameId, cancellationToken);
        if (game is not null)
        {
            _logger.LogError("Game report for GameID: {GameId} already exists in the database.", gameId);
            return (false, new GameReportResult(false, "This game was already reported", $"A game report for game with ID {gameId} already exists in the database."));
        }

        return (true, new GameReportResult());
    }
}
