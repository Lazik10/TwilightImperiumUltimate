using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.MainStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerGameStatsFactory
{
    Task<AsyncPlayerMainStatsSummaryDto> CreateAsyncPlayerGameStats(AsyncPlayerProfile playerProfile);
}
