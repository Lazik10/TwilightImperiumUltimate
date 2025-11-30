using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.API.Discord.Services;

public interface ILeaderPublisher
{
    Task<bool> PublishEmojiAsync(LeaderLog log, CancellationToken cancellationToken);
    Task<bool> PublishTextAsync(LeaderLog log, CancellationToken cancellationToken);
}
