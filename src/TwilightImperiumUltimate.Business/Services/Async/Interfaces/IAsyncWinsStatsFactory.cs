using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncWinsStatsFactory
{
    Task<AsyncWinsSummaryStatsDto> CreateAsyncWinsStatsSummary(int limit, CancellationToken cancellationToken);
}
