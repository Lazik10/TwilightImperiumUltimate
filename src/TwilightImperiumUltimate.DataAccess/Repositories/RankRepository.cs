using FluentResults;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class RankRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<RankRepository> logger)
    : IRankRepository
{
    public async Task<Result<int>> AddRank(int tiglUserId, TiglRankName rankName, TiglLeague league, long timestamp, int matchId, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var tiglUser = await dbContext.TiglUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == tiglUserId, cancellationToken);

        var tiglUserName = tiglUser?.TiglUserName ?? string.Empty;
        var tiglUserDiscordId = tiglUser?.DiscordId ?? 0;

        var newRank = new TiglRank
        {
            TiglUserId = tiglUserId,
            Name = rankName,
            League = league,
            AchievedAt = timestamp,
        };

        var currentRank = await GetCurrentRank(tiglUserId, league, cancellationToken);

        dbContext.Ranks.Add(newRank);
        dbContext.RankUpLogs.Add(CreateRankUpLog(
            tiglUserId,
            timestamp,
            tiglUserDiscordId,
            tiglUserName,
            currentRank.Value.Name,
            rankName,
            league,
            currentRank.IsSuccess ? timestamp - currentRank.Value.AchievedAt : 0,
            matchId));

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(newRank.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding rank {RankName} for user ID: {TiglUserId} and League {League}", rankName, tiglUserId, league);
            return Result.Fail($"Error adding rank {rankName} for user ID: {tiglUserId} and League {league}: {ex.Message}");
        }
    }

    public async Task<Result<TiglRank>> GetCurrentRank(int tiglUserId, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var currentRank = await dbContext.Ranks
            .Where(x => x.TiglUserId == tiglUserId && x.League == league)
            .OrderByDescending(x => x.AchievedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (currentRank is not null)
            return Result.Ok(currentRank);

        logger.LogError("Current rank not found for user ID: {TiglUserId} and League {League}", tiglUserId, league);
        return Result.Fail($"Current rank not found for user ID: {tiglUserId} and League {league}!");
    }

    public async Task<Result<TiglRankName>> GetPreMatchRank(int tiglUserId, long matchStart, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var preMatchRank = await dbContext.Ranks
            .Where(r => r.TiglUserId == tiglUserId && r.League == league && r.AchievedAt <= matchStart)
            .OrderByDescending(r => r.AchievedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (preMatchRank is not null)
            return Result.Ok(preMatchRank.Name);

        logger.LogError("Prematch rank not found for user ID: {TiglUserId} and League {League}", tiglUserId, league);
        return Result.Fail($"Prematch rank not found for user ID: {tiglUserId} and League {league}!");
    }

    public async Task<Result<bool>> UpdateUserAndMatchStats(MatchReport matchReport, TiglUser player, AsyncPlayerMatchStats asyncPlayerMatchStats, GlickoPlayerMatchStats glickoPlayerMatchStats, TrueSkillPlayerMatchStats trueSkillMatchStats, CancellationToken cancellationToken)
    {
        var dbContext = await context.CreateDbContextAsync(cancellationToken);

        dbContext.TiglUsers.Update(player);

        dbContext.AsyncPlayerMatchStats.Update(asyncPlayerMatchStats);
        dbContext.GlickoPlayerMatchStats.Update(glickoPlayerMatchStats);
        dbContext.TrueSkillPlayerMatchStats.Update(trueSkillMatchStats);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving rank up changes for user {UserId} and user name {UserName} in game {MatchReportId}", player.Id, player.DiscordId, matchReport.GameId);
            return Result.Fail($"Error saving rank up changes for user {player.Id} and user name {player.DiscordId} in game {matchReport.GameId}: {ex.Message}");
        }

        return Result.Ok(true);
    }

    private RankUpLog CreateRankUpLog(int tiglUserId, long timestamp, long tiglUserDiscordId, string tiglUserName, TiglRankName oldRank, TiglRankName newRank, TiglLeague league, long duration, int matchId)
    {
        return new RankUpLog
        {
            Timestamp = timestamp,
            League = league,
            OldRank = oldRank,
            NewRank = newRank,
            TiglUserId = tiglUserId,
            TiglUserDiscordId = tiglUserDiscordId,
            TiglUserName = tiglUserName,
            Duration = duration,
            MatchId = matchId,
        };
    }
}
