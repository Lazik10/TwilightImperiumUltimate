using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Web.Models.Async;

namespace TwilightImperiumUltimate.Web.Services.Async;

public interface IAsyncFactionMinMaxStatsProvider
{
    Task<AsyncFactionMinMaxValues> GetCorrectStatValues(
        AsyncFactionsMaxStatValues maxStats,
        AsyncFactionStatsByGameVpDto factionStats,
        FactionStatisticsSubstatsFilter statsFilter);
}
