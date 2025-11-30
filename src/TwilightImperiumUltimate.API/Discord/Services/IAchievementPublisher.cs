using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.API.Discord.Services;

public interface IAchievementPublisher
{
    Task<bool> PublishAsync(AchievementLog log, CancellationToken cancellationToken);
}
