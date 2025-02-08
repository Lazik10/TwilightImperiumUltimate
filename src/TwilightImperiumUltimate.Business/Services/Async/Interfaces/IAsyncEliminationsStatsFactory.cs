using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncEliminationsStatsFactory
{
    Task<AsyncEliminationsSummaryStatsDto> CreateAsyncEliminationsStatsSummary(int limit, CancellationToken cancellationToken);
}
