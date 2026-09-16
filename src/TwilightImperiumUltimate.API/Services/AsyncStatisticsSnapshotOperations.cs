using Quartz;
using TwilightImperiumUltimate.API.Jobs;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.API.Services;

public sealed class AsyncStatisticsSnapshotOperations(
    IAsyncStatisticsSnapshotRepository repository,
    ISchedulerFactory schedulerFactory)
    : IAsyncStatisticsSnapshotOperations
{
    private readonly IAsyncStatisticsSnapshotRepository _repository = repository;
    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;

    public Task<AsyncStatisticsSnapshot?> GetPublishedAsync(CancellationToken cancellationToken)
    {
        return _repository.GetPublishedAsync(cancellationToken);
    }

    public async Task RequestRefreshAsync(CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.TriggerJob(new JobKey(nameof(AsyncStatisticsSnapshotJob)), cancellationToken);
    }
}
