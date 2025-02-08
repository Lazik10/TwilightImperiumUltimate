using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.HitStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerCombatStatsFactory
{
    Task<AsyncPlayerCombatStatsSummaryDto> CreateAsyncPlayerCombatStats(AsyncPlayerProfile playerProfile);
}
