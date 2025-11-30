using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Core.Entities.Statistics;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class AchievementRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<GameStatistics> logger)
    : IAchievementRepository
{
    public async Task AwardFactionAchievement(int tiglUserId, MatchReport matchReport, AchievementName achievementName, TiglFactionName faction, int minWins, CancellationToken cancellationToken)
    {
        await using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var achievement = await dbContext.Achievements
            .FirstOrDefaultAsync(a => a.Name == achievementName, cancellationToken);

        var tiglUser = await dbContext.TiglUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(tu => tu.Id == tiglUserId, cancellationToken);

        if (achievement is null || tiglUser is null)
            return;

        var hasAchievement = await dbContext.TiglUserAchievements
            .AnyAsync(
                tua => tua.TiglUserId == tiglUserId
                && tua.AchievementId == achievement.Id
                && tua.Faction == faction,
                cancellationToken);

        if (hasAchievement)
            return;

        var factionWins = await dbContext.GameReports
            .Where(x => x.EndTimestamp <= matchReport.StartTimestamp && (x.League == TiglLeague.ThundersEdge || x.League == TiglLeague.Fractured))
            .Include(x => x.PlayerResults)
            .SelectMany(gr => gr.PlayerResults)
            .CountAsync(pr => pr.Faction == faction && pr.TiglUserId == tiglUserId && pr.IsWinner, cancellationToken);

        if (factionWins < minWins)
            return;

        var newAchievement = new TiglUserAchievement
        {
            TiglUserId = tiglUserId,
            AchievementId = achievement.Id,
            AchievementName = achievement.Name,
            Faction = faction,
            AchievedAt = matchReport.EndTimestamp,
            MatchId = matchReport.Id,
            MatchName = matchReport.GameId,
        };

        dbContext.AchievementLogs.Add(CreateAchievementLog(matchReport.EndTimestamp, achievementName, faction, tiglUserId, tiglUser.DiscordTag, tiglUser.DiscordId, matchReport.Id));
        dbContext.TiglUserAchievements.Add(newAchievement);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation(ex, "Achievement {AchievementName} already awarded to user {TiglUserId}", achievementName, tiglUserId);
        }
    }

    private AchievementLog CreateAchievementLog(long timestamp, AchievementName achievement, TiglFactionName faction, int tiglUserId, string tiglUserName, long tiglUserDiscordId, int matchId = 0)
    {
        return new AchievementLog
        {
            Timestamp = timestamp,
            AchievementName = achievement,
            Faction = faction,
            TiglUserId = tiglUserId,
            TiglUserName = tiglUserName,
            TiglUserDiscordId = tiglUserDiscordId,
            Published = false,
            MatchId = matchId,
        };
    }

    public async Task AwardAchievement(int tiglUserId, MatchReport matchReport, AchievementName achievementName, CancellationToken cancellationToken, TiglFactionName faction = TiglFactionName.None)
    {
        await using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var achievement = await dbContext.Achievements
            .FirstOrDefaultAsync(a => a.Name == achievementName, cancellationToken);

        var tiglUser = await dbContext.TiglUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(tu => tu.Id == tiglUserId, cancellationToken);

        if (achievement is null || tiglUser is null)
            return;

        var hasAchievement = await dbContext.TiglUserAchievements
            .AnyAsync(tua => tua.TiglUserId == tiglUserId && tua.AchievementId == achievement.Id, cancellationToken);

        if (hasAchievement)
            return;

        var newAchievement = new TiglUserAchievement
        {
            TiglUserId = tiglUserId,
            AchievementId = achievement.Id,
            AchievementName = achievement.Name,
            Faction = faction,
            AchievedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            MatchId = matchReport.Id,
            MatchName = matchReport.GameId,
        };

        dbContext.AchievementLogs.Add(CreateAchievementLog(matchReport.EndTimestamp, achievementName, faction, tiglUserId, tiglUser.DiscordTag, tiglUser.DiscordId, matchReport.Id));
        dbContext.TiglUserAchievements.Add(newAchievement);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation(ex, "Achievement {AchievementName} already awarded to user {TiglUserId}", achievementName, tiglUserId);
        }
    }

    public async Task<List<TiglUserAchievement>> GetUserAchievements(int tiglUserId, CancellationToken cancellationToken)
    {
        await using var dbContext = await context.CreateDbContextAsync(cancellationToken);
        return await dbContext.TiglUserAchievements
            .Where(tua => tua.TiglUserId == tiglUserId)
            .Include(tua => tua.Achievement)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TiglUserAchievement>> GetUsersAchievements(IEnumerable<int> tiglUserIds, CancellationToken cancellationToken)
    {
        await using var dbContext = await context.CreateDbContextAsync(cancellationToken);
        return await dbContext.TiglUserAchievements
            .Where(tua => tiglUserIds.Contains(tua.TiglUserId))
            .Include(tua => tua.Achievement)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TiglUserAchievement>> GetRecentAchievements(int take, CancellationToken cancellationToken)
    {
        await using var dbContext = await context.CreateDbContextAsync(cancellationToken);
        return await dbContext.TiglUserAchievements
            .Include(tua => tua.Achievement)
            .Include(tua => tua.TiglUser)
            .OrderByDescending(tua => tua.AchievedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AddManualAchievement(int tiglUserId, AchievementName achievementName, TiglFactionName faction, CancellationToken cancellationToken)
    {
        await using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var achievement = await dbContext.Achievements.FirstOrDefaultAsync(a => a.Name == achievementName, cancellationToken);
        if (achievement is null)
            return false;

        var exists = await dbContext.TiglUserAchievements.AnyAsync(a => a.TiglUserId == tiglUserId && a.AchievementId == achievement.Id && a.Faction == faction, cancellationToken);
        if (exists)
            return true;

        var entity = new TiglUserAchievement
        {
            TiglUserId = tiglUserId,
            AchievementId = achievement.Id,
            AchievementName = achievement.Name,
            Faction = faction,
            AchievedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            MatchId = 0,
            MatchName = string.Empty,
        };

        dbContext.TiglUserAchievements.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoveAchievement(int tiglUserId, AchievementName achievementName, TiglFactionName faction, CancellationToken cancellationToken)
    {
        await using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var achievement = await dbContext.Achievements.FirstOrDefaultAsync(a => a.Name == achievementName, cancellationToken);
        if (achievement is null)
            return false;

        var existing = await dbContext.TiglUserAchievements.FirstOrDefaultAsync(a => a.TiglUserId == tiglUserId && a.AchievementId == achievement.Id && a.Faction == faction, cancellationToken);
        if (existing is null)
            return true;

        dbContext.TiglUserAchievements.Remove(existing);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
