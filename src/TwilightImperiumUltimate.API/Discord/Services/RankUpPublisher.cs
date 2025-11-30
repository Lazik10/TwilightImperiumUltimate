using FluentResults;
using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Tigl.Discord;

namespace TwilightImperiumUltimate.API.Discord.Services;

internal sealed class RankUpPublisher(IDiscordClient discordClient, ILogger<RankUpPublisher> logger) : IRankUpPublisher
{
    private readonly CompositeFormat _resetRankToUnrankedFmt =
        CompositeFormat.Parse("<@{0}>'s rank has been reset to rank **{1}**. Good luck with your next climb!");

    private readonly CompositeFormat _rankUpFormatFmt =
        CompositeFormat.Parse("<@{0}> has been promoted to rank **{1}** after {2}!");

    private readonly List<TiglRankName> standardRanks = new()
    {
        TiglRankName.Minister,
        TiglRankName.Agent,
        TiglRankName.Commander,
        TiglRankName.Hero,
    };

    private readonly List<TiglRankName> fracturedRanks = new()
    {
        TiglRankName.Thrall,
        TiglRankName.Acolyte,
        TiglRankName.Legionnaire,
        TiglRankName.Starlancer,
        TiglRankName.GeneSorcerer,
        TiglRankName.IxthLord,
        TiglRankName.Archon,
    };

    public async Task<bool> PublishAsync(RankUpLog log, CancellationToken cancellationToken)
    {
        var content = BuildRankUpContent(log);

        // Publish the rank-up notification in discord channel
        var notificationPublished = await PublishRankUpAsync(log.League, content, cancellationToken);
        if (!notificationPublished)
            logger.LogWarning("Failed to publish rank-up log {LogId}", log.Id);

        // Add new role to user
        if (log.NewRank != TiglRankName.Unranked)
        {
            var addRoleSuccess = await AddRoleAsync(log, cancellationToken);
            if (!addRoleSuccess)
                logger.LogWarning("Failed to add role for user {UserId} to rank {Rank}", log.TiglUserDiscordId, log.NewRank);
        }
        else if (log.OldRank == TiglRankName.Hero || log.OldRank == TiglRankName.Archon)
        {
            // Remove roles from user so they can rank up again
            var removeRolesSuccess = await RemoveRolesAsync(log, cancellationToken);
            if (!removeRolesSuccess)
                logger.LogWarning("Failed to remove roles for user {UserId} when resetting rank to Unranked", log.TiglUserDiscordId);
        }

        return notificationPublished;
    }

    private static string GetRankText(TiglRankName rank)
        => DiscordRoleMappings.RankRoles.TryGetValue(rank, out var roleInfo) ? roleInfo.RoleName : rank.ToString();

    private static string GetDuration(long durationMs)
    {
        var time = TimeSpan.FromMilliseconds(durationMs);

        var parts = new List<string>(3);

        if (time.Days > 0)
            parts.Add($"{time.Days:D2} d");
        if (time.Hours > 0)
            parts.Add($"{time.Hours:D2} h");
        if (time.Minutes > 0)
            parts.Add($"{time.Minutes:D2} m");
        if (parts.Count == 0)
        {
            var secs = (int)Math.Floor(time.TotalSeconds);
            return $"{secs}s";
        }

        return string.Join(" ", parts);
    }

    private string BuildRankUpContent(RankUpLog log)
    {
        var sb = new StringBuilder();

        if (log.NewRank == TiglRankName.Unranked)
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, _resetRankToUnrankedFmt, log.TiglUserDiscordId, GetRankText(log.NewRank)));
        else
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, _rankUpFormatFmt, log.TiglUserDiscordId, GetRankText(log.NewRank), GetDuration(log.Duration)));

        return sb.ToString();
    }

    private Task<bool> PublishRankUpAsync(TiglLeague league, string content, CancellationToken cancellationToken)
        => league switch
        {
            TiglLeague.Fractured => discordClient.PostRankUpFracturedAsync(content, cancellationToken),
            TiglLeague.ProphecyOfKings or TiglLeague.ThundersEdge => discordClient.PostRankUpStandardAsync(content, cancellationToken),
            _ => Task.FromResult(false),
        };

    private async Task<bool> AddRoleAsync(RankUpLog log, CancellationToken cancellationToken)
    {
        DiscordRoleMappings.RankRoles.TryGetValue(log.NewRank, out var roleInfo);
        if (roleInfo is null)
            return false;

        await discordClient.AddRoleToUserAsync((ulong)log.TiglUserDiscordId, (ulong)long.Parse(roleInfo.RoleId, CultureInfo.InvariantCulture), DiscordRoleType.Rank, cancellationToken);
        return true;
    }

    private async Task<bool> RemoveRolesAsync(RankUpLog log, CancellationToken cancellationToken)
    {
        var rolesToRemove = new List<TiglRankName>();
        var allSuccess = true;

        if (log.OldRank == TiglRankName.Hero)
            rolesToRemove = standardRanks;
        else if (log.OldRank == TiglRankName.Archon)
            rolesToRemove = fracturedRanks;

        foreach (var rank in rolesToRemove)
        {
            if (rank <= log.OldRank && DiscordRoleMappings.RankRoles.TryGetValue(rank, out var roleInfo))
            {
                var result = await discordClient.RemoveRoleFromUserAsync((ulong)log.TiglUserDiscordId, (ulong)long.Parse(roleInfo.RoleId, CultureInfo.InvariantCulture), DiscordRoleType.Rank, cancellationToken);
                if (!result)
                {
                    allSuccess = false;
                    logger.LogWarning("Failed to remove rank role {RankRole} from user {UserId}", rank, log.TiglUserName);
                }
            }
        }

        return allSuccess;
    }
}
