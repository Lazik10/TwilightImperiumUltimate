using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncFactionStatsFactory
{
    Task<AsyncFactionsSummaryStatsDto> CreateAsyncFactionStatsSummary(CancellationToken cancellationToken);
}
