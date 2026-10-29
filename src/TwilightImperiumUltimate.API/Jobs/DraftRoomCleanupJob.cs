using Quartz;
using TwilightImperiumUltimate.Business.Services.DraftRooms;

namespace TwilightImperiumUltimate.API.Jobs;

/// <summary>Runs inactive-room retention without expiring finished result snapshots.</summary>
[DisallowConcurrentExecution]
public sealed class DraftRoomCleanupJob(DraftRoomCleanupService cleanup) : IJob
{
    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        await cleanup.CleanupAsync(context.CancellationToken);
    }
}
