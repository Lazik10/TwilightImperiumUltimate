using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncGamesStatsFactory
{
    Task<AsyncGamesSummaryStatsDto> CreateAsyncGamesStatsSummary(int limit, CancellationToken cancellationToken);
}
