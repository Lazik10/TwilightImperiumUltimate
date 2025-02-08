using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncHistoryStatsFactory
{
    Task<AsyncHistorySummaryStatsDto> CreateAsyncHistoryStatsSummary(CancellationToken cancellationToken);
}
