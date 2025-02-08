using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncTurnsStatsFactory
{
    Task<AsyncTurnsSummaryStatsDto> CreateAsyncTurnsStatsSummary(int limit, CancellationToken cancellationToken);
}
