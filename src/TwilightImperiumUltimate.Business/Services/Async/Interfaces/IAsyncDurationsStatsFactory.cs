using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncDurationsStatsFactory
{
    Task<AsyncDurationsSummaryStatsDto> CreateAsyncDurationsStatsSummary(int limit, CancellationToken cancellationToken);
}
