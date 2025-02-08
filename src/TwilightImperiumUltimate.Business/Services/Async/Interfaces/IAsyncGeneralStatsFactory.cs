using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncGeneralStatsFactory
{
    Task<AsyncGeneralSummaryStatsDto> CreateAsyncGeneralStatsSummary(CancellationToken cancellationToken);
}
