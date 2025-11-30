using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Tigl.Discord;

namespace TwilightImperiumUltimate.API.Discord.Services;

internal sealed class LeaderPublisher(IDiscordClient discordClient, ILogger<LeaderPublisher> logger) : ILeaderPublisher
{
    private readonly CompositeFormat _leaderStolenStdFmt =
        CompositeFormat.Parse("<@{0}> has taken **{1}** from <@{2}> after {3}!");

    private readonly CompositeFormat _leaderNewStdFmt =
        CompositeFormat.Parse("<@{0}> has claimed **{1}** — this hero has been claimed for the first time!");

    private readonly CompositeFormat _leaderStolenFracFmt =
        CompositeFormat.Parse("<@{0}> has taken **{1}** from <@{2}> after {3}!");

    private readonly CompositeFormat _leaderNewFracFmt =
        CompositeFormat.Parse("<@{0}> has claimed **{1}** — this hero has been claimed for the first time!");

    public Task<bool> PublishEmojiAsync(LeaderLog log, CancellationToken cancellationToken)
    {
        var role = DiscordRoleMappings.PrestigeRoles.GetValueOrDefault(log.Name);
        var emoji = role?.EmojiId ?? string.Empty;

        if (string.IsNullOrEmpty(emoji))
            return Task.FromResult(false);

        return PublishInternalAsync(log.League, $"<:{emoji}>", cancellationToken);
    }

    public async Task<bool> PublishTextAsync(LeaderLog log, CancellationToken cancellationToken)
    {
        var (_, text) = BuildLeaderContent(log);

        var notificationPublished = await PublishInternalAsync(log.League, text, cancellationToken);
        if (!notificationPublished)
            logger.LogWarning("Failed to publish leader change log with ID: {LogId}", log.Id);

        var removeRolesSuccess = await RemoveRolesAsync(log, cancellationToken);
        if (!removeRolesSuccess)
            logger.LogWarning("Failed to remove hero role for user {UserId}", log.PreviousOwnerDiscordId);

        var addRoleSuccess = await AddRoleAsync(log, cancellationToken);
        if (!addRoleSuccess)
            logger.LogWarning("Failed to add hero role for user {UserId}", log.NewOwnerDiscordId);

        return notificationPublished;
    }

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

    private (string EmojiFirst, string Text) BuildLeaderContent(LeaderLog log)
    {
        var role = DiscordRoleMappings.PrestigeRoles.GetValueOrDefault(log.Name);
        var leaderId = log.Name.ToString();

        if (role is not null)
        {
            if (role.RoleId.Length <= 1)
                leaderId = role.RoleName;
            else
                leaderId = $"<@&{role.RoleId}>";
        }

        var prevOwner = log.PreviousOwnerId > 0 ? log.PreviousOwnerDiscordId.ToString(CultureInfo.InvariantCulture) : string.Empty;

        var sb = new StringBuilder();
        if (log.PreviousOwnerId > 0)
        {
            if (log.League is TiglLeague.ProphecyOfKings or TiglLeague.ThundersEdge)
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, LeaderMessage(true, true), log.NewOwnerDiscordId, leaderId, prevOwner, GetDuration(log.Duration)));
            else
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, LeaderMessage(false, true), log.NewOwnerDiscordId, leaderId, prevOwner, GetDuration(log.Duration)));
        }
        else
        {
            if (log.League is TiglLeague.ProphecyOfKings or TiglLeague.ThundersEdge)
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, LeaderMessage(true, false), log.NewOwnerDiscordId, leaderId));
            else
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, LeaderMessage(false, false), log.NewOwnerDiscordId, leaderId));
        }

        return (role?.EmojiId ?? string.Empty, sb.ToString());
    }

    private CompositeFormat LeaderMessage(bool standard, bool stolen)
        => (standard, stolen) switch
        {
            (true, true) => _leaderStolenStdFmt,
            (true, false) => _leaderNewStdFmt,
            (false, true) => _leaderStolenFracFmt,
            _ => _leaderNewFracFmt,
        };

    private Task<bool> PublishInternalAsync(TiglLeague league, string content, CancellationToken cancellationToken)
        => league switch
        {
            TiglLeague.Fractured => discordClient.PostLeadersFracturedAsync(content, cancellationToken),
            TiglLeague.ProphecyOfKings or TiglLeague.ThundersEdge => discordClient.PostLeadersStandardAsync(content, cancellationToken),
            _ => Task.FromResult(false),
        };

    private async Task<bool> AddRoleAsync(LeaderLog log, CancellationToken cancellationToken)
    {
        DiscordRoleMappings.PrestigeRoles.TryGetValue(log.Name, out var roleInfo);
        if (roleInfo is null || roleInfo.RoleId.Length <= 1)
            return false;

        return await discordClient.AddRoleToUserAsync((ulong)log.NewOwnerDiscordId, (ulong)long.Parse(roleInfo.RoleId, CultureInfo.InvariantCulture), DiscordRoleType.Leader, cancellationToken);
    }

    private async Task<bool> RemoveRolesAsync(LeaderLog log, CancellationToken cancellationToken)
    {
        DiscordRoleMappings.PrestigeRoles.TryGetValue(log.Name, out var roleInfo);
        if (roleInfo is null || roleInfo.RoleId.Length <= 1)
            return false;

        return await discordClient.RemoveRoleFromUserAsync((ulong)log.PreviousOwnerDiscordId, (ulong)long.Parse(roleInfo.RoleId, CultureInfo.InvariantCulture), DiscordRoleType.Leader, cancellationToken);
    }
}
