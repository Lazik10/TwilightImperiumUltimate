using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Helpers;
using TwilightImperiumUltimate.Web.Models.Async;

namespace TwilightImperiumUltimate.Web.Services.Async;

public class AsyncFactionMinMaxStatsProvider : IAsyncFactionMinMaxStatsProvider
{
    public Task<AsyncFactionMinMaxValues> GetCorrectStatValues(
        AsyncFactionsMaxStatValues maxStats,
        AsyncFactionStatsByGameVpDto factionStats,
        FactionStatisticsSubstatsFilter statsFilter)
    {
        AsyncFactionMinMaxValues result = statsFilter switch
        {
            FactionStatisticsSubstatsFilter.Games => new(factionStats.GetFloatValue(x => x.Games), maxStats.Games < 1 ? 1 : maxStats.Games),
            FactionStatisticsSubstatsFilter.Wins => new(factionStats.GetFloatValue(x => x.Wins), maxStats.Wins < 1 ? 1 : maxStats.Wins),
            FactionStatisticsSubstatsFilter.WinPercentage => new(factionStats.GetFloatValue(x => x.WinsPercentage), maxStats.WinPercentage < 1 ? 100f : maxStats.WinPercentage),
            FactionStatisticsSubstatsFilter.Eliminations => new(0f, factionStats.GetFloatValue(x => x.Eliminations), maxStats.Eliminations < 1f ? 1 : maxStats.Eliminations),
            FactionStatisticsSubstatsFilter.AverageVp => new(5f, factionStats.GetFloatValue(x => x.Games == 0 ? 0 : (float)x.Vp / x.Games), maxStats.AverageVp < 1f ? 1 : maxStats.AverageVp),
            FactionStatisticsSubstatsFilter.AverageVpPercentage => new(50f, factionStats.GetFloatValue(x => x.VpPercentage), maxStats.AverageVpPrecentage < 1 ? 100f : maxStats.AverageVpPrecentage),
            _ => new(),
        };

        result.Games = factionStats.Games;

        return Task.FromResult(result);
    }
}
