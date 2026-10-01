using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IAsyncStatisticsSnapshotRepository
{
    Task<AsyncStatisticsSnapshot?> GetPublishedAsync(CancellationToken cancellationToken);

    Task<AsyncStatisticsSnapshot> PublishAsync(
        DateTime generatedAtUtc,
        string? sourceDataVersion,
        string payload,
        CancellationToken cancellationToken);
    }
