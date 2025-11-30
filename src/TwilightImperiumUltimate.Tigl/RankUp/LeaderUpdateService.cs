using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal class LeaderUpdateService(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<PrestigeRankService> logger)
    : ILeaderUpdateService
{
    public async Task<bool> UpdateLeader(TiglUser player, MatchReport matchReport, TiglFactionName faction, CancellationToken cancellationToken)
    {
        using var dbContext = await context.CreateDbContextAsync(cancellationToken);
        var league = matchReport.League;

        var leader = await dbContext.Leaders
            .Include(x => x.CurrentOwner)
            .FirstOrDefaultAsync(x => x.League == league && x.Faction == faction, cancellationToken);

        if (leader is null)
        {
            logger.LogError("Unable to retriev Leader: {Leader} information from database!", faction);
            return false;
        }

        var now = matchReport.EndTimestamp;
        var previousUpdate = leader.LastUpdate ?? 0;
        var duration = 0L;

        var previousOwnerId = leader.CurrentOwnerId ?? 0;
        var previousOwnerName = leader.CurrentOwner?.TiglUserName ?? string.Empty;
        var previousOwnerDiscordId = leader.CurrentOwner?.DiscordId ?? 0;

        if (leader.ChangeCount == 0)
        {
            leader.FirstUpdate = now;
            leader.ShortestDuration = 0;
            leader.LongestDuration = 0;
            leader.PreviousOwnerId = null;
            leader.CurrentOwnerId = player.Id;
        }
        else
        {
            duration = previousUpdate == 0 ? 0 : now - previousUpdate;

            if (leader.ShortestDuration == 0 || duration < leader.ShortestDuration)
            {
                leader.ShortestDuration = duration;
                leader.ShortestHolderId = player.Id;
            }

            if (leader.LongestDuration == 0 || duration > leader.LongestDuration)
            {
                leader.LongestDuration = duration;
                leader.LongestHolderId = player.Id;
            }

            leader.PreviousOwnerId = previousOwnerId;
            leader.CurrentOwner = null;
            leader.CurrentOwnerId = player.Id;
        }

        leader.ChangeCount++;
        leader.LastUpdate = now;
        leader.PreviousUpdate = previousUpdate;

        dbContext.LeaderLogHistory.Add(CreateLeaderLog(player, previousOwnerId, previousOwnerDiscordId, previousOwnerName, leader.Name, league, faction, now, duration, matchReport.Id));
        dbContext.Update(leader);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating leader {Leader} in league {League}", faction, league);
            return false;
        }
    }

    private LeaderLog CreateLeaderLog(
        TiglUser player,
        int previousOwnerId,
        long previousOwnerDiscordId,
        string previousOwnerName,
        TiglPrestigeRank name,
        TiglLeague league,
        TiglFactionName faction,
        long timestamp,
        long duration,
        int matchId)
    {
        return new LeaderLog
        {
            Name = name,
            Timestamp = timestamp,
            League = league,
            Faction = faction,
            PreviousOwnerId = previousOwnerId,
            PreviousOwnerDiscordId = previousOwnerDiscordId,
            PreviousOwner = previousOwnerName,
            NewOwnerId = player.Id,
            NewOwnerDiscordId = player.DiscordId,
            NewOwner = player.TiglUserName,
            Duration = duration,
            MatchId = matchId,
        };
    }
}
