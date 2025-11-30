using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Tigl.Discord;

namespace TwilightImperiumUltimate.API.Discord.Services;

internal sealed class AchievementPublisher(IDiscordClient discordClient, ILogger<AchievementPublisher> logger) : IAchievementPublisher
{
    private static readonly CompositeFormat AchievementFormat = CompositeFormat.Parse("<@{0}> earned achievement **{1}** - {2}! {3}");

    public async Task<bool> PublishAsync(AchievementLog log, CancellationToken cancellationToken)
    {
        var descriptor = log.AchievementName.GetDescriptor();

        var name = descriptor.DisplayName;
        var details = descriptor.Details;
        var faction = log.Faction == TiglFactionName.None ? string.Empty : $"({TiglFactionParser.ToFactionString(log.Faction)})";
        var content = string.Format(CultureInfo.InvariantCulture, AchievementFormat, log.TiglUserDiscordId, name, details, faction);

        var ok = await discordClient.PostAchievementsAsync(content, cancellationToken);
        if (!ok)
            logger.LogWarning("Failed to publish achievement log {LogId}", log.Id);

        return ok;
    }
}
