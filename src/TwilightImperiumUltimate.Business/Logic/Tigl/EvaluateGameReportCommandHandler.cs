using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Tigl.Achievements;
using TwilightImperiumUltimate.Tigl.AsyncRating;
using TwilightImperiumUltimate.Tigl.Glicko2Rating;
using TwilightImperiumUltimate.Tigl.RankUp;
using TwilightImperiumUltimate.Tigl.TrueSkillRating;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public partial class EvaluateGameReportCommandHandler(
    ITiglRepository tiglRepository,
    ITiglUserRepository tiglUserRepository,
    IAsyncRatingProcessor asyncRatingProcessor,
    IGlickoRatingProcessor glickoRatingProcessor,
    ITrueSkillRatingProcessor trueSkillRatingProcessor,
    IRankUpResolver rankUpResolver,
    IAchievementService achievementService,
    IDbContextFactory<TwilightImperiumDbContext> dbContextFactory,
    ILogger<EvaluateGameReportCommandHandler> logger)
    : IRequestHandler<EvaluateGameReportCommand, GameReportResult>
{
    private readonly ILogger<EvaluateGameReportCommandHandler> _logger = logger;

    public async Task<GameReportResult> Handle(EvaluateGameReportCommand request, CancellationToken cancellationToken)
    {
        var matchReport = await tiglRepository.GetMatchReportWithPlayerResults(request.MatchReportId, cancellationToken);
        if (matchReport is null)
        {
            _logger.LogWarning("Match report with id {MatchReportId} not found for evaluation.", request.MatchReportId);
            return new GameReportResult(false, "Match Not Found", "Could not locate match report to evaluate.");
        }

        if (matchReport.State != MatchState.New && matchReport.State != MatchState.AddedInQueue)
        {
            _logger.LogWarning("Match report {MatchReportId} already evaluated or in invalid state {State}.", request.MatchReportId, matchReport.State);
            return new GameReportResult(false, "Invalid State", "Match report already processed or cannot be evaluated.");
        }

        // Adapter converting domain MatchReport to IGameReport expected by processors
        var adapter = new GameReportAdapter(matchReport);
        await UpdateAdapterWithUserDiscordData(adapter, matchReport, tiglUserRepository, cancellationToken);

        var asyncSuccess = await asyncRatingProcessor.ProcessGameReport(adapter, matchReport.Id, cancellationToken);
        var glickoSuccess = await glickoRatingProcessor.ProcessGameReport(adapter, matchReport.Id, cancellationToken);
        var trueSkillSuccess = await trueSkillRatingProcessor.ProcessGameReport(adapter, matchReport.Id, cancellationToken);

        if (asyncSuccess && glickoSuccess && trueSkillSuccess && matchReport.Source == ResultSource.Async)
        {
            await rankUpResolver.ResolveRankUp(matchReport.Id, matchReport.League, cancellationToken);
        }

        if (matchReport.League == TiglLeague.ThundersEdge || matchReport.League == TiglLeague.Fractured)
        {
            await achievementService.AddGame(matchReport.Id, cancellationToken);
        }

        var updateResult = await tiglRepository.UpdateMatch(matchReport.Id, cancellationToken);
        if (updateResult.IsFailed)
        {
            _logger.LogError("Failed to update match state after evaluation for match {MatchReportId}", matchReport.Id);
            return new GameReportResult(false, "Failed To Update", "Game evaluated but failed to update match state.");
        }

        // Enqueue for unified log publishing
        try
        {
            await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            db.GamePublishLogs.Add(new GamePublishLog
            {
                MatchId = matchReport.Id,
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
            _logger.LogError(ex, "Failed to enqueue match {MatchId} for log publishing", matchReport.Id);
        }

        return new GameReportResult();
    }

    private async Task UpdateAdapterWithUserDiscordData(GameReportAdapter adapter, Core.Entities.Tigl.MatchReport matchReport, ITiglUserRepository tiglUserRepository, CancellationToken cancellationToken)
    {
        var playerResultsList = new List<PlayerResult>();
        foreach (var playerResult in matchReport.PlayerResults)
        {
            var tiglUser = await tiglUserRepository.GetTiglUserById(playerResult.TiglUserId, matchReport.League, cancellationToken);
            if (tiglUser != null)
            {
                playerResultsList.Add(new PlayerResult
                {
                    DiscordId = tiglUser.DiscordId,
                    DiscordTag = tiglUser.DiscordTag,
                    Faction = TiglFactionParser.ToFactionString(playerResult.Faction),
                    Score = playerResult.Score,
                    IsWinner = playerResult.IsWinner,
                });
            }
        }

        adapter.PlayerResults = playerResultsList;
    }
}
