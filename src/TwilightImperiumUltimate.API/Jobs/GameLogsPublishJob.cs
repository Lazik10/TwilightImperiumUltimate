using Quartz;
using TwilightImperiumUltimate.API.Discord.Services;

namespace TwilightImperiumUltimate.API.Jobs;

internal class GameLogsPublishJob(IGameLogPublishWorkflow workflow, ILogger<GameLogsPublishJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await workflow.PublishNextAsync(context.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GameLogsPublishJob failed");
        }
    }
}
