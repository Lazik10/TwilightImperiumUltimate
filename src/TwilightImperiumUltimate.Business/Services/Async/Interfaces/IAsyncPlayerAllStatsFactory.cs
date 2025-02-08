using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerAllStatsFactory
{
    Task<AsyncPlayerProfileSummaryStatsDto> CreateAsyncPlayerStats(AsyncPlayerProfile playerProfile);

    Task<AsyncPlayerProfileSummaryStatsDto> CreateDefaultAsyncPlayerStats();
}
