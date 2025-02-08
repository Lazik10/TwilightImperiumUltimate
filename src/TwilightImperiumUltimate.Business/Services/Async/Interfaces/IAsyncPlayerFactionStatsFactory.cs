using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerFactionStatsFactory
{
    Task<AsyncPlayerFactionStatsSummaryDto> CreateAsyncPlayerFactionStats(AsyncPlayerProfile playerProfile);
}