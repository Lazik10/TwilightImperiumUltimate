using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal class PrestigeRankService(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<PrestigeRankService> logger)
    : IPrestigeRankService
{
    public async Task<int> AddFactionPrestigeRank(int userId, MatchReport matchReport, TiglFactionName faction, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var tiglUser = await GetTiglUserName(dbContext, userId, cancellationToken);

        var rank = await dbContext.PrestigeRanks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FactionName == faction && x.League == league, cancellationToken);

        if (rank is null)
            return -1;

        var prestigeRank = new TiglUserPrestigeRank
        {
            TiglUserId = userId,
            PrestigeRankId = rank.Id,
            AchievedAt = matchReport.EndTimestamp,
        };

        if (tiglUser is not null)
        {
            dbContext.TiglUserPrestigeRanks.Add(prestigeRank);
            dbContext.PrestigeLogs.Add(CreatePrestigeLog(userId, matchReport.EndTimestamp, tiglUser, rank.Name, faction, league, 0, matchReport.Id));

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving prestige rank up changes for user {UserId} and faction {Faction} in league {League}", userId, faction, league);
                return -1;
            }
        }

        return rank.Id;
    }

    public async Task<int> AddGalacticThreatRank(int userId, MatchReport matchReport, int rank, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var tiglUser = await GetTiglUserName(dbContext, userId, cancellationToken);

        var galacticThreatRank = await dbContext.PrestigeRanks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == TiglPrestigeRank.GalacticThreat && x.League == league, cancellationToken);

        var galacticThreatRanks = await dbContext.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TiglUserId == userId && x.PrestigeRank.Name == TiglPrestigeRank.GalacticThreat && x.PrestigeRank.League == league && x.Rank == rank, cancellationToken);

        if (galacticThreatRank is null || galacticThreatRanks is not null)
            return -1;

        var prestigeRank = new TiglUserPrestigeRank
        {
            TiglUserId = userId,
            PrestigeRankId = galacticThreatRank.Id,
            AchievedAt = matchReport.EndTimestamp,
            Rank = rank,
        };

        if (tiglUser is not null)
        {
            dbContext.TiglUserPrestigeRanks.Add(prestigeRank);
            dbContext.PrestigeLogs.Add(CreatePrestigeLog(userId, matchReport.EndTimestamp, tiglUser, TiglPrestigeRank.GalacticThreat, TiglFactionName.None, league, rank, matchReport.Id));

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving galactic threat rank up changes for user {UserId} and rank {Rank}", userId, rank);
                return -1;
            }
        }

        return galacticThreatRank.Id;
    }

    public async Task<int> AddPaxMagnificaBellumGloriosumRank(int userId, MatchReport matchReport, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var tiglUser = await GetTiglUserName(dbContext, userId, cancellationToken);

        var paxMagnificaBellumGloriosumRank = await dbContext.PrestigeRanks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == TiglPrestigeRank.PaxMagnificaBellumGloriosum && x.League == league, cancellationToken);

        var paxMagnificaPlayerRank = await dbContext.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TiglUserId == userId && x.PrestigeRank.Name == TiglPrestigeRank.PaxMagnificaBellumGloriosum && x.PrestigeRank.League == league, cancellationToken);

        if (paxMagnificaBellumGloriosumRank is null || paxMagnificaPlayerRank is not null)
            return -1;

        var prestigeRank = new TiglUserPrestigeRank
        {
            TiglUserId = userId,
            PrestigeRankId = paxMagnificaBellumGloriosumRank.Id,
            AchievedAt = matchReport.EndTimestamp,
        };

        if (tiglUser is not null)
        {
            dbContext.TiglUserPrestigeRanks.Add(prestigeRank);
            dbContext.PrestigeLogs.Add(CreatePrestigeLog(userId, matchReport.EndTimestamp, tiglUser, TiglPrestigeRank.PaxMagnificaBellumGloriosum, TiglFactionName.None, league, 0, matchReport.Id));

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving Pax Magnifica Bellum Gloriosum rank up changes for user {UserId}", userId);
                return -1;
            }
        }

        return paxMagnificaBellumGloriosumRank.Id;
    }

    public async Task<int> AddImperiumRank(int userId, MatchReport matchReport, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        var tiglUser = await GetTiglUserName(dbContext, userId, cancellationToken);

        var imperiumRank = await dbContext.PrestigeRanks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == TiglPrestigeRank.Tyrant && x.League == TiglLeague.Fractured, cancellationToken);

        var playerImperiumRank = await dbContext.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .AsNoTracking()
            .OrderByDescending(imperiumRank => imperiumRank.Rank)
            .FirstOrDefaultAsync(x => x.TiglUserId == userId && x.PrestigeRank.Name == TiglPrestigeRank.Tyrant && x.PrestigeRank.League == TiglLeague.Fractured, cancellationToken);

        if (imperiumRank is null)
            return -1;

        var prestigeRank = new TiglUserPrestigeRank
        {
            TiglUserId = userId,
            PrestigeRankId = imperiumRank.Id,
            AchievedAt = matchReport.EndTimestamp,
            Rank = playerImperiumRank is not null ? playerImperiumRank.Rank + 1 : 1,
        };

        if (tiglUser is not null)
        {
            dbContext.TiglUserPrestigeRanks.Add(prestigeRank);
            dbContext.PrestigeLogs.Add(CreatePrestigeLog(userId, matchReport.EndTimestamp, tiglUser, TiglPrestigeRank.Tyrant, TiglFactionName.None, TiglLeague.Fractured, prestigeRank.Rank, matchReport.Id));

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving Imperium rank up changes for user {UserId}", userId);
                return -1;
            }
        }

        return imperiumRank.Id;
    }

    public async Task<int> GetFactionPrestigeRankCount(int userId, MatchReport matchReport, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        return await dbContext.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .Where(x => x.PrestigeRank.FactionName != TiglFactionName.None)
            .CountAsync(
                x => x.TiglUserId == userId
                && x.PrestigeRank.League == league
                && x.AchievedAt <= matchReport.EndTimestamp,
                cancellationToken);
    }

    public async Task<bool> HasFactionPrestigeRank(int userId, MatchReport matchReport, TiglFactionName faction, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        return await dbContext.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .AnyAsync(
                x => x.TiglUserId == userId
                && x.PrestigeRank.FactionName == faction
                && x.PrestigeRank.League == league
                && x.AchievedAt <= matchReport.EndTimestamp,
                cancellationToken);
    }

    public async Task<bool> HasGalacticThreatRank(int userId, MatchReport matchReport, int rank, TiglLeague league, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);

        return await dbContext.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .AnyAsync(
                x => x.TiglUserId == userId
                && x.PrestigeRank.Name == TiglPrestigeRank.GalacticThreat
                && x.Rank == rank
                && x.PrestigeRank.League == league
                && x.AchievedAt <= matchReport.EndTimestamp,
                cancellationToken);
    }

    private static async Task<TiglUser?> GetTiglUserName(TwilightImperiumDbContext dbContext, int userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.TiglUsers.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken: cancellationToken);
        return user;
    }

    private PrestigeLog CreatePrestigeLog(int userId, long timestamp, TiglUser tiglUser, TiglPrestigeRank prestigeRank, TiglFactionName faction, TiglLeague league, int rank = 0, int matchId = 0)
    {
        return new PrestigeLog
        {
            Timestamp = timestamp,
            Name = prestigeRank,
            FactionName = faction,
            League = league,
            Rank = rank,
            TiglUserId = userId,
            TiglUserDiscordId = tiglUser.DiscordId,
            TiglUserName = tiglUser.TiglUserName,
            MatchId = matchId,
        };
    }
}
