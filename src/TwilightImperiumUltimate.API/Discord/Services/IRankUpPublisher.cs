using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.API.Discord.Services;

public interface IRankUpPublisher
{
    Task<bool> PublishAsync(RankUpLog log, CancellationToken cancellationToken);
}
