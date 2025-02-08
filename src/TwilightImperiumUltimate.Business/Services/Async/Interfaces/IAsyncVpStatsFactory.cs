using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncVpStatsFactory
{
    Task<AsyncVpSummaryStatsDto> CreateAsyncVpStatsSummary(int limit, CancellationToken cancellationToken);
}
