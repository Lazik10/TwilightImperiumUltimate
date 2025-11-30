namespace TwilightImperiumUltimate.API.Discord.Services;

public interface IGameLogPublishWorkflow
{
    Task PublishNextAsync(CancellationToken cancellationToken);
}
