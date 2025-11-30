using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements;

public class AchievementService(
    IServiceScopeFactory serviceScopeFactory,
    IDbContextFactory<TwilightImperiumDbContext> dbContextFactory,
    ILogger<AchievementService> logger)
    : IAchievementService
{
    private static readonly ConcurrentQueue<int> _queue = new();
    private static readonly SemaphoreSlim _sem = new(1, 1);

    public Task AddGame(int gameId, CancellationToken cancellationToken = default)
    {
        _queue.Enqueue(gameId);
        return Task.CompletedTask;
    }

    public async Task EvaluatePendingAsync(CancellationToken cancellationToken = default)
    {
        if (!await _sem.WaitAsync(TimeSpan.Zero, cancellationToken))
            return;

        try
        {
            while (_queue.TryDequeue(out var gameDbId))
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var discoveryScope = serviceScopeFactory.CreateScope())
                {
                    var matchReport = await GetMatchReport(gameDbId, discoveryScope, cancellationToken);
                    if (matchReport is null)
                        continue;

                    var achievementRepository = discoveryScope.ServiceProvider.GetRequiredService<IAchievementRepository>();

                    var playerIds = matchReport.PlayerResults.Select(pr => pr.TiglUserId).Distinct().ToList();
                    var userAchievements = await achievementRepository.GetUsersAchievements(playerIds, cancellationToken);
                    var evaluatorInstances = discoveryScope.ServiceProvider.GetServices<IAchievementEvaluator>().ToList();

                    // First evaluate post-match achievements
                    await EvaluatePostMatchAchievements(matchReport, userAchievements, evaluatorInstances, cancellationToken);

                    userAchievements.Clear();
                    userAchievements = await achievementRepository.GetUsersAchievements(playerIds, cancellationToken);

                    // Then evaluate summary achievements
                    await EvaluateSummaryAchievements(matchReport, userAchievements, evaluatorInstances, cancellationToken);

                    try
                    {
                        using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                        var gamePublishLog = await dbContext.GamePublishLogs.FirstOrDefaultAsync(gpl => gpl.MatchId == matchReport.Id, cancellationToken);

                        if (gamePublishLog != null)
                        {
                            gamePublishLog.AchievementsEvaluated = true;
                            dbContext.GamePublishLogs.Update(gamePublishLog);
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to save achievement evaluations for game {GameId}", matchReport.GameId);
                    }
                }
            }
        }
        finally
        {
            _sem.Release();
        }
    }

    public async Task EvaluateEndOfSeasonAchievements(Season season, CancellationToken cancellationToken)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var evaluatorInstances = scope.ServiceProvider.GetServices<IEndOfSeasonAchievementEvaluator>().ToList();

            var endOfSeasonAchievements = evaluatorInstances
                .Select(e => e.GetType())
                .Distinct()
                .Select(t => new { Type = t, Attribute = t.GetCustomAttributes(typeof(AchievementEndOfSeasonEvaluatorAttribute), false).Cast<AchievementEndOfSeasonEvaluatorAttribute>().FirstOrDefault() })
                .Where(x => x.Attribute is not null);

            foreach (var achievement in endOfSeasonAchievements)
            {
                var evaluator = evaluatorInstances.First(e => e.GetType() == achievement.Type);
                await evaluator.EvaluateAsync(season, achievement.Attribute!.Name, cancellationToken);
            }
        }
    }

    private async Task EvaluateSummaryAchievements(MatchReport matchReport, List<TiglUserAchievement> userAchievements, List<IAchievementEvaluator> evaluatorInstances, CancellationToken cancellationToken)
    {
        var evaluatorTypesWithAttributes = evaluatorInstances
            .Select(e => e.GetType())
            .Distinct()
            .Select(t => new { Type = t, Attribute = t.GetCustomAttributes(typeof(AchievementSummaryEvaluatorAttribute), false).Cast<AchievementSummaryEvaluatorAttribute>().FirstOrDefault() })
            .Where(x => x.Attribute is not null)
            .ToList();

        var postEvaluationTasks = evaluatorTypesWithAttributes.Select(async x =>
        {
            using var evalScope = serviceScopeFactory.CreateScope();
            var evaluator = (IAchievementEvaluator)evalScope.ServiceProvider.GetRequiredService(x.Type);
            try
            {
                await evaluator.EvaluateAsync(matchReport, x.Attribute!.Name, userAchievements, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Achievement evaluator {Evaluator} failed for game {GameId}", evaluator.GetType().Name, matchReport.GameId);
            }
        });

        await Task.WhenAll(postEvaluationTasks);
    }

    private async Task EvaluatePostMatchAchievements(MatchReport matchReport, List<TiglUserAchievement> userAchievements, List<IAchievementEvaluator> evaluatorInstances, CancellationToken cancellationToken)
    {
        var evaluatorTypesWithAttributes = evaluatorInstances
            .Select(e => e.GetType())
            .Distinct()
            .Select(t => new { Type = t, Attribute = t.GetCustomAttributes(typeof(AchievementEvaluatorAttribute), false).Cast<AchievementEvaluatorAttribute>().FirstOrDefault() })
            .Where(x => x.Attribute is not null)
            .ToList();

        var evaluationTasks = evaluatorTypesWithAttributes.Select(async x =>
        {
            using var evalScope = serviceScopeFactory.CreateScope();
            var evaluator = (IAchievementEvaluator)evalScope.ServiceProvider.GetRequiredService(x.Type);
            try
            {
                await evaluator.EvaluateAsync(matchReport, x.Attribute!.Name, userAchievements, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Achievement evaluator {Evaluator} failed for game {GameId}", evaluator.GetType().Name, matchReport.GameId);
            }
        });

        await Task.WhenAll(evaluationTasks);
    }

    private async Task<MatchReport?> GetMatchReport(int gameDbId, IServiceScope discoveryScope, CancellationToken cancellationToken)
    {
        var repo = discoveryScope.ServiceProvider.GetRequiredService<ITiglRepository>();
        var matchReport = await repo.GetMatchReportById(gameDbId, cancellationToken);
        if (matchReport is null)
        {
            logger.LogWarning("MatchReport with DB Id {GameDbId} not found for achievement evaluation.", gameDbId);
            return null;
        }

        return matchReport;
    }
}
