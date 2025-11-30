using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.API.Discord.Services;

public interface IPrestigePublisher
{
    Task<bool> PublishAsync(PrestigeLog log, CancellationToken cancellationToken);
}
