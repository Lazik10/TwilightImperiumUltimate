using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.Tigl.Discord;

namespace TwilightImperiumUltimate.API.Discord.Services;

internal sealed class PrestigePublisher(
    IDbContextFactory<TwilightImperiumDbContext> dbContextFactory,
    IDiscordClient discordClient,
    ILogger<PrestigePublisher> logger) : IPrestigePublisher
{
    private const int ThundersEdgePrestigeRanksPerCycle = 5;

    private readonly CompositeFormat _prestigeFormatFmt = CompositeFormat.Parse("<@{0}> has achieved prestige rank **{1}**{2}!");

    public async Task<bool> PublishAsync(PrestigeLog log, CancellationToken cancellationToken)
    {
        var content = await BuildPrestigeContentAsync(log, cancellationToken);

        var ok = await PublishAsyncInternal(log.League, content, cancellationToken);
        if (!ok)
            logger.LogWarning("Failed to publish prestige log {LogId}", log.Id);

        var roleAssigned = await AssignGlobalPrestigeRoleAsync(log, cancellationToken);
        if (!roleAssigned)
            logger.LogWarning("Failed to assign prestige role {PrestigeRank} to user {UserId}", log.Name, log.TiglUserDiscordId);

        return ok;
    }

    private static async Task<long?> GetThundersEdgePrestigeIntervalStartAsync(
        TwilightImperiumDbContext dbContext,
        int tiglUserId,
        PrestigeLog log,
        int factionPrestigeRankCount,
        CancellationToken cancellationToken)
    {
        if (factionPrestigeRankCount % ThundersEdgePrestigeRanksPerCycle != 1)
        {
            return await dbContext.TiglUserPrestigeRanks
                .AsNoTracking()
                .Where(prestigeRank => prestigeRank.TiglUserId == tiglUserId
                    && prestigeRank.PrestigeRank.FactionName != TiglFactionName.None
                    && prestigeRank.PrestigeRank.League == log.League
                    && prestigeRank.AchievedAt < log.Timestamp)
                .OrderByDescending(prestigeRank => prestigeRank.AchievedAt)
                .Select(prestigeRank => (long?)prestigeRank.AchievedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await dbContext.Ranks
            .AsNoTracking()
            .Where(rank => rank.TiglUserId == tiglUserId
                && rank.League == log.League
                && rank.Name == TiglRankName.Hero
                && rank.AchievedAt < log.Timestamp)
            .OrderByDescending(rank => rank.AchievedAt)
            .Select(rank => (long?)rank.AchievedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<bool> AssignGlobalPrestigeRoleAsync(PrestigeLog log, CancellationToken cancellationToken)
    {
        if (log.Name is not (TiglPrestigeRank.GalacticThreat or TiglPrestigeRank.PaxMagnificaBellumGloriosum or TiglPrestigeRank.Tyrant))
            return true;

        if (!DiscordRoleMappings.PrestigeRoles.TryGetValue(log.Name, out var roleInfo)
            || !ulong.TryParse(roleInfo.RoleId, NumberStyles.None, CultureInfo.InvariantCulture, out var roleId)
            || roleId == 0)
        {
            return false;
        }

        var correctRoleId = log.Name switch
        {
            TiglPrestigeRank.GalacticThreat => DiscordRoleMappings.GetCorrectGalacticThreatPrestigeRankRoleId(log.Rank),
            TiglPrestigeRank.Tyrant => DiscordRoleMappings.GetCorrectTyrantPrestigeRankRoleId(log.Rank),
            _ => roleId,
        };

        return await discordClient.AddRoleToUserAsync(
            (ulong)log.TiglUserDiscordId,
            correctRoleId,
            DiscordRoleType.Leader,
            cancellationToken);
    }

    private Task<bool> PublishAsyncInternal(TiglLeague league, string content, CancellationToken cancellationToken)
        => league switch
        {
            TiglLeague.Fractured => discordClient.PostRankUpFracturedAsync(content, cancellationToken),
            TiglLeague.ProphecyOfKings or TiglLeague.ThundersEdge => discordClient.PostRankUpStandardAsync(content, cancellationToken),
            _ => Task.FromResult(false),
        };

    private async Task<string> BuildPrestigeContentAsync(PrestigeLog log, CancellationToken cancellationToken)
    {
        var rankText = DiscordRoleMappings.PrestigeRoles.TryGetValue(log.Name, out var roleInfo) ? roleInfo.RoleName : log.Name.ToString();
        if (log.Name == TiglPrestigeRank.Tyrant || log.Name == TiglPrestigeRank.GalacticThreat)
            rankText += $" #{log.Rank}";

        var factionPrestigeRankCountText = string.Empty;
        if (log.FactionName != TiglFactionName.None && log.TiglUserId is int tiglUserId)
        {
            var factionPrestigeRankCount = await GetFactionPrestigeRankCountAsync(tiglUserId, log, cancellationToken);
            factionPrestigeRankCountText = $" (**{factionPrestigeRankCount}**)";

            if (log.League == TiglLeague.ThundersEdge)
            {
                var factionGameCount = await GetFactionGameCountAsync(tiglUserId, log, factionPrestigeRankCount, cancellationToken);
                if (factionGameCount is not null)
                    factionPrestigeRankCountText += $" Games: {factionGameCount.Value}";
            }
        }

        return string.Format(CultureInfo.InvariantCulture, _prestigeFormatFmt, log.TiglUserDiscordId, rankText, factionPrestigeRankCountText);
    }

    private async Task<int> GetFactionPrestigeRankCountAsync(int tiglUserId, PrestigeLog log, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var factionPrestigeRanks = await dbContext.TiglUserPrestigeRanks
            .Where(prestigeRank => prestigeRank.TiglUserId == tiglUserId
                && prestigeRank.PrestigeRank.FactionName != TiglFactionName.None
                && prestigeRank.PrestigeRank.League == log.League
                && prestigeRank.AchievedAt <= log.Timestamp)
            .Select(prestigeRank => prestigeRank.PrestigeRank.FactionName)
            .ToListAsync(cancellationToken);

        return log.League == TiglLeague.ThundersEdge
            ? factionPrestigeRanks.Select(faction => faction.GetPrestigeCountIdentity()).Distinct().Count()
            : factionPrestigeRanks.Count;
    }

    private async Task<int?> GetFactionGameCountAsync(int tiglUserId, PrestigeLog log, int factionPrestigeRankCount, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var games = dbContext.GameReports
            .AsNoTracking()
            .Where(match => match.League == log.League
                && match.EndTimestamp <= log.Timestamp);

        var intervalStart = await GetThundersEdgePrestigeIntervalStartAsync(dbContext, tiglUserId, log, factionPrestigeRankCount, cancellationToken);
        if (intervalStart is null)
            return null;

        return await games
            .Where(match => match.EndTimestamp > intervalStart.Value
                && match.PlayerResults.Any(player => player.TiglUserId == tiglUserId))
            .CountAsync(cancellationToken);
    }
}
