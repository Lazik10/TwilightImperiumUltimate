using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Tigl.Discord;

namespace TwilightImperiumUltimate.API.Discord.Services;

internal sealed class PrestigePublisher(IDiscordClient discordClient, ILogger<PrestigePublisher> logger) : IPrestigePublisher
{
    private readonly CompositeFormat _prestigeFormatFmt = CompositeFormat.Parse("<@{0}> has achieved prestige rank **{1}**!");

    public async Task<bool> PublishAsync(PrestigeLog log, CancellationToken cancellationToken)
    {
        var content = BuildPrestigeContent(log);

        var ok = await PublishAsyncInternal(log.League, content, cancellationToken);
        if (!ok)
            logger.LogWarning("Failed to publish prestige log {LogId}", log.Id);

        var roleAssigned = await AssignGlobalPrestigeRoleAsync(log, cancellationToken);
        if (!roleAssigned)
            logger.LogWarning("Failed to assign prestige role {PrestigeRank} to user {UserId}", log.Name, log.TiglUserDiscordId);

        return ok;
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

    private string BuildPrestigeContent(PrestigeLog log)
    {
        var rankText = DiscordRoleMappings.PrestigeRoles.TryGetValue(log.Name, out var roleInfo) ? roleInfo.RoleName : log.Name.ToString();
        if (log.Name == TiglPrestigeRank.Tyrant || log.Name == TiglPrestigeRank.GalacticThreat)
            rankText += $" #{log.Rank}";

        return string.Format(CultureInfo.InvariantCulture, _prestigeFormatFmt, log.TiglUserDiscordId, rankText);
    }
}
