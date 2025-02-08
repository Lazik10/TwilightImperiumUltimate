using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncCombatStatsFactory
{
    Task<AsyncCombatSummaryStatsDto> CreateAsyncCombatStatsSummary(int limit, CancellationToken cancellationToken);
}
