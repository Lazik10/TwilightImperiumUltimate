using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.API.Discord.Services;

public interface IDiscordRoleChangePublisher
{
    Task<bool> PublishLogToDatabase(DiscordRoleChangeLog log);
}
