using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncStatisticsSnapshotBuilder
{
    Task<AsyncStatisticsSnapshotDto> BuildAsync(CancellationToken cancellationToken);
}
