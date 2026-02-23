using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Options;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Tigl.Achievements;
using TwilightImperiumUltimate.Tigl.AsyncRating;
using TwilightImperiumUltimate.Tigl.Glicko2Rating;
using TwilightImperiumUltimate.Tigl.RankUp;
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
    IRankUpResolver rankUpResolver,
    IAchievementService achievementService,
    IDbContextFactory<TwilightImperiumDbContext> dbContextFactory,
    IOptions<TiglOptions> tiglOptions,
    ILogger<ReportGameCommandHandler> logger)
    : IRequestHandler<ReportGameCommand, GameReportResult>
{
    private const int MinimumRequiredPlayerCount = 6;
    private readonly ILogger<ReportGameCommandHandler> _logger = logger;
    private readonly TiglOptions _tiglOptions = tiglOptions.Value;

    public async Task<GameReportResult> Handle(ReportGameCommand request, CancellationToken cancellationToken)
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
            return new GameReportResult(false, "Game is too old", $"Game {gameReport.GameId} started on {gameReport.StartTimestamp.ToDateOnly()} before the allowed cutoff date of 1st December 2025 and cannot be reported.");
        }

        var onlyImportEnabled = await tiglRepository.GetTiglParameter(TiglParameterName.OnlyImportReports, cancellationToken);
        if (onlyImportEnabled is not null && onlyImportEnabled.Enabled && gameReport.Source != ResultSource.Test)
        {
            return new GameReportResult(false, "Only Import Reports Enabled", "The system is currently set that only imported reports can be evaluated. Please contact @lazik2110 for help.");
        }
        else if (onlyImportEnabled is not null && onlyImportEnabled.Enabled && gameReport.Source == ResultSource.Test)
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
            var stringBuilder = new StringBuilder();
            foreach (var error in factionValidationResult.Errors)
                stringBuilder.AppendLine(error.Message);

            return new GameReportResult(false, "Invalid Factions", stringBuilder.ToString());
        }

        var usersExist = await tiglMatchUserValidator.AllTiglUsersExists(gameReport, cancellationToken);
        if (!usersExist)
        {
            var registerNewUsersResult = await tiglMatchUserValidator.RegisterNewTiglUsers(gameReport, cancellationToken);
            if (registerNewUsersResult.IsFailed)
            {
                var stringBuilder = new StringBuilder();
                foreach (var error in registerNewUsersResult.Errors)
                    stringBuilder.AppendLine(error.Message);
                return new GameReportResult(false, "Failed to register player(s)", stringBuilder.ToString());
            }
        }

        var scoreValidationResult = await tiglResultValidator.ValidateResult(gameReport);
        if (!scoreValidationResult)
            return new GameReportResult(false, "Invalid Player Results", $"No player reached required points for game: {gameReport.GameId}. Please report game manually or contact @lazik2110");

        var insertResult = await matchInserter.InsertGameReport(gameReport, cancellationToken);
        if (insertResult.IsFailed)
        {
            foreach (var error in insertResult.Errors)
                _logger.LogError("Insert game report error: {InsertError}", error.Message);
            return new GameReportResult { Success = false, ErrorTitle = $"Insertion of results for game {gameReport.GameId} failed" };
        }
        else
        {
            var gameId = insertResult.Value.Id;
            var asyncSuccess = await asyncRatingProcessor.ProcessGameReport(gameReport, gameId, cancellationToken);
            var glickoSuccess = await glickoRatingProcessor.ProcessGameReport(gameReport, gameId, cancellationToken);
            var trueSkillSuccess = await trueSkillRatingProcessor.ProcessGameReport(gameReport, gameId, cancellationToken);

            if (asyncSuccess && glickoSuccess && trueSkillSuccess && gameReport.Source == ResultSource.Async)
                await rankUpResolver.ResolveRankUp(gameId, gameReport.League, cancellationToken);

            if (gameReport.League == TiglLeague.ThundersEdge || gameReport.League == TiglLeague.Fractured)
                await achievementService.AddGame(gameId, cancellationToken);
        }

        var matchUpdateResult = await tiglRepository.UpdateMatch(insertResult.Value.Id, cancellationToken);
        if (matchUpdateResult.IsFailed)
        {
            _logger.LogError("Failed to update match for GameID: {GameId}", gameReport.GameId);
            return new GameReportResult(false, "Failed to mark game as processed", $"There was an error when marking game {gameReport.GameId} as processed in the database!");
        }

        try
        {
            await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            db.GamePublishLogs.Add(new GamePublishLog
            {
                MatchId = insertResult.Value.Id,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Published = false,
                RankUpPublish = false,
                PrestigePublish = false,
                LeaderPublish = false,
                AchievementPublish = false,
            });
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enqueue match {MatchId} for log publishing", insertResult.Value.Id);
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
        return (false, new GameReportResult(false, "Invalid Player Count", $"Not enough player results in the game report for game {gameReport.GameId}. If one or more player(s) got eliminated please report this game manually!"));
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
