using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerTurnStatsFactory
{
    Task<AsyncPlayerTurnStatsSummaryDto> CreateAsyncPlayerTurnStats(AsyncPlayerProfile playerProfile);
}
