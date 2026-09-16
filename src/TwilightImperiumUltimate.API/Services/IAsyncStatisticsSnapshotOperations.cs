using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.API.Services;

public interface IAsyncStatisticsSnapshotOperations
{
    Task<AsyncStatisticsSnapshot?> GetPublishedAsync(CancellationToken cancellationToken);

    Task RequestRefreshAsync(CancellationToken cancellationToken);
}
