using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Web.Models.Async;

namespace TwilightImperiumUltimate.Web.Services.Async;

public interface IAsyncPlayerMinMaxStatsProvider
{
    Task<AsyncPlayerFactionMinMaxValues> GetCorrectStatValues(
        AsyncPlayerMaxStatValues maxStats,
        AsyncPlayerFactionStatsByGameVp factionStats,
        FactionStatisticsSubstatsFilter statsFilter);
}
