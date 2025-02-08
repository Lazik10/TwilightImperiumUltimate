using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Web.Helpers;
using TwilightImperiumUltimate.Web.Models.Async;

namespace TwilightImperiumUltimate.Web.Services.Async;

public class AsyncPlayerMinMaxStatsProvider : IAsyncPlayerMinMaxStatsProvider
{
    public Task<AsyncPlayerFactionMinMaxValues> GetCorrectStatValues(
        AsyncPlayerMaxStatValues maxStats,
        AsyncPlayerFactionStatsByGameVp factionStats,
        FactionStatisticsSubstatsFilter statsFilter)
    {
        AsyncPlayerFactionMinMaxValues result = statsFilter switch
        {
            FactionStatisticsSubstatsFilter.Games => new(factionStats.GetFloatValue(x => x.Games), maxStats.Games < 1 ? 1 : maxStats.Games),
            FactionStatisticsSubstatsFilter.Wins => new(factionStats.GetFloatValue(x => x.Wins), maxStats.Wins < 1 ? 1 : maxStats.Wins),
            FactionStatisticsSubstatsFilter.WinPercentage => new(factionStats.GetFloatValue(x => x.WinRate), maxStats.WinPercentage < 1 ? 100f : maxStats.WinPercentage),
            FactionStatisticsSubstatsFilter.Eliminations => new(0f, factionStats.GetFloatValue(x => x.Eliminations), maxStats.Eliminations < 1f ? 1 : maxStats.Eliminations),
            FactionStatisticsSubstatsFilter.MinVp => new(factionStats.GetFloatValue(x => x.MinVp), maxStats.MinVp < 1 ? 1 : maxStats.MinVp),
            FactionStatisticsSubstatsFilter.AverageVp => new(factionStats.GetFloatValue(x => x.AverageVp), maxStats.AverageVp < 1 ? 1 : maxStats.AverageVp),
            FactionStatisticsSubstatsFilter.MaxVp => new(factionStats.GetFloatValue(x => x.MaxVp), maxStats.MaxVp < 1 ? 1 : maxStats.MaxVp),
            FactionStatisticsSubstatsFilter.AverageVpPercentage => new(factionStats.GetFloatValue(x => x.AverageVpPercentage), maxStats.AverageVpPrecentage < 1 ? 100f : maxStats.AverageVpPrecentage),
            _ => new(),
        };

        result.Games = factionStats.Games;

        return Task.FromResult(result);
    }
}
