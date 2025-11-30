using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.Tigl.Achievements;

namespace TwilightImperiumUltimate.API.Discord.Services;

internal sealed class GameLogPublishWorkflow(
    IDbContextFactory<TwilightImperiumDbContext> dbContextFactory,
    IAchievementService achievementService,
    IRankUpPublisher rankUpPublisher,
    IPrestigePublisher prestigePublisher,
    ILeaderPublisher leaderPublisher,
    IAchievementPublisher achievementPublisher,
    ILogger<GameLogPublishWorkflow> logger) : IGameLogPublishWorkflow
{
    public async Task PublishNextAsync(CancellationToken cancellationToken)
    {
        await achievementService.EvaluatePendingAsync(cancellationToken);

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var cutoff = DateTimeOffset.UtcNow.AddMinutes(-1).ToUnixTimeMilliseconds();

        var item = await db.GamePublishLogs
            .Where(q => !q.Published && q.CreatedAt < cutoff && q.AchievementsEvaluated && !q.PublishingInProgress)
            .OrderBy(q => q.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (item is null)
            return;

        var isDiscordPublishAllowed = await db.TiglParameters.FirstOrDefaultAsync(x => x.Name == TiglParameterName.DiscordInteraction, cancellationToken);
        if (isDiscordPublishAllowed is not null && !isDiscordPublishAllowed.Enabled)
        {
            item.AchievementPublish = true;
            item.RankUpPublish = true;
            item.PrestigePublish = true;
            item.LeaderPublish = true;
            item.Published = true;

            await db.GamePublishLogs.Where(l => l.Id == item.Id)
                .ExecuteUpdateAsync(
                    s => s
                    .SetProperty(l => l.AchievementPublish, true)
                    .SetProperty(l => l.RankUpPublish, true)
                    .SetProperty(l => l.PrestigePublish, true)
                    .SetProperty(l => l.LeaderPublish, true)
                    .SetProperty(l => l.Published, true),
                    cancellationToken);

            logger.LogInformation("Skipping publishing for match {MatchId} because Discord publishing is disabled.", item.MatchId);
            return;
        }

        await db.GamePublishLogs.Where(l => l.Id == item.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(l => l.PublishingInProgress, true), cancellationToken);

        var rankUpDone = item.RankUpPublish;
        var prestigeDone = item.PrestigePublish;
        var leaderDone = item.LeaderPublish;
        var achievementDone = item.AchievementPublish;

        if (!rankUpDone)
            rankUpDone = await PublishRankUpsAsync(db, item.MatchId, cancellationToken);

        if (!prestigeDone)
            prestigeDone = await PublishPrestigeAsync(db, item.MatchId, cancellationToken);

        if (!leaderDone)
            leaderDone = await PublishLeadersAsync(db, item.MatchId, cancellationToken);

        if (!achievementDone)
            achievementDone = await PublishAchievementsAsync(db, item.MatchId, cancellationToken);

        var publishedDone = rankUpDone && prestigeDone && leaderDone && achievementDone;

        await db.GamePublishLogs.Where(l => l.Id == item.Id)
            .ExecuteUpdateAsync(
                s => s
                .SetProperty(l => l.RankUpPublish, rankUpDone)
                .SetProperty(l => l.PrestigePublish, prestigeDone)
                .SetProperty(l => l.LeaderPublish, leaderDone)
                .SetProperty(l => l.AchievementPublish, achievementDone)
                .SetProperty(l => l.Published, publishedDone)
                .SetProperty(l => l.PublishingInProgress, false),
                cancellationToken);
    }

    private async Task<bool> PublishRankUpsAsync(TwilightImperiumDbContext db, int matchId, CancellationToken cancellationToken)
    {
        var logs = await db.RankUpLogs.Where(l => l.MatchId == matchId && !l.Published).OrderBy(l => l.Timestamp).ToListAsync(cancellationToken);
        if (logs.Count == 0)
            return true;

        var ids = new List<int>(logs.Count);

        foreach (var log in logs)
        {
            if (await rankUpPublisher.PublishAsync(log, cancellationToken))
                ids.Add(log.Id);
        }

        if (ids.Count > 0)
        {
            await db.RankUpLogs.Where(l => ids.Contains(l.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.Published, true), cancellationToken);
        }

        return true;
    }

    private async Task<bool> PublishPrestigeAsync(TwilightImperiumDbContext db, int matchId, CancellationToken ct)
    {
        var logs = await db.PrestigeLogs.Where(l => l.MatchId == matchId && !l.Published).OrderBy(l => l.Timestamp).ToListAsync(ct);
        if (logs.Count == 0)
            return true;

        var ids = new List<int>(logs.Count);

        foreach (var log in logs)
        {
            if (await prestigePublisher.PublishAsync(log, ct))
                ids.Add(log.Id);
        }

        if (ids.Count > 0)
        {
            await db.PrestigeLogs.Where(l => ids.Contains(l.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.Published, true), ct);
        }

        return true;
    }

    private async Task<bool> PublishLeadersAsync(TwilightImperiumDbContext db, int matchId, CancellationToken ct)
    {
        var logs = await db.LeaderLogHistory.Where(l => l.MatchId == matchId && !l.Published).OrderBy(l => l.Timestamp).ToListAsync(ct);
        if (logs.Count == 0)
            return true;

        var ids = new List<int>(logs.Count);

        foreach (var log in logs)
        {
            await leaderPublisher.PublishEmojiAsync(log, ct);
            if (await leaderPublisher.PublishTextAsync(log, ct))
                ids.Add(log.Id);
        }

        if (ids.Count > 0)
        {
            await db.LeaderLogHistory.Where(l => ids.Contains(l.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.Published, true), ct);
        }

        return true;
    }

    private async Task<bool> PublishAchievementsAsync(TwilightImperiumDbContext db, int matchId, CancellationToken ct)
    {
        var logs = await db.AchievementLogs.Where(l => l.MatchId == matchId && !l.Published).OrderBy(l => l.Timestamp).ToListAsync(ct);
        if (logs.Count == 0)
            return true;

        var ids = new List<int>(logs.Count);

        foreach (var log in logs)
        {
            if (await achievementPublisher.PublishAsync(log, ct))
                ids.Add(log.Id);
        }

        if (ids.Count > 0)
        {
            await db.AchievementLogs.Where(l => ids.Contains(l.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.Published, true), ct);
        }

        return true;
    }
}
