using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerVpStatsFactory
{
    Task<AsyncPlayerVpStatsSummaryDto> CreateAsyncPlayerVpStats(AsyncPlayerProfile playerProfile);
}
