using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncOpponentsStatsFactory
{
    Task<AsyncOpponentsSummaryStatsDto> CreateAsyncOpponentsStatsSummary(int limit, CancellationToken cancellationToken);
}
