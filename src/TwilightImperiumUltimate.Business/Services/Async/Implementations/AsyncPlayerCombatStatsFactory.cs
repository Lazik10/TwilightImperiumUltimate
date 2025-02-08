using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.HitStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerCombatStatsFactory : IAsyncPlayerCombatStatsFactory
{
    public Task<AsyncPlayerCombatStatsSummaryDto> CreateAsyncPlayerCombatStats(AsyncPlayerProfile playerProfile)
    {
        var games = playerProfile.GameStatistics
            .Select(x => x.GameStats)
            .Where(x => x.HasWinner && x.EndedTimestamp != null)
            .ToList();

        var tiglGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var totalCombatStats = GetCombatStats(games, playerProfile.DiscordUserId);
        var tiglCombatStats = GetCombatStats(tiglGames, playerProfile.DiscordUserId);
        var customCombatStats = GetCombatStats(customGames, playerProfile.DiscordUserId);

        return Task.FromResult(new AsyncPlayerCombatStatsSummaryDto(totalCombatStats, tiglCombatStats, customCombatStats));
    }

    private AsyncPlayerCombatStatsDto GetCombatStats(List<GameStats> gameStats, long discordUserId)
    {
        var playerStats = gameStats.SelectMany(x => x.PlayerStatistics)
            .Where(x => x.DiscordUserID == discordUserId)
            .ToList();

        if (playerStats.Count > 0)
        {
            var expectedHits = playerStats.Sum(x => x.ExpectedHits);
            var actualHits = playerStats.Sum(x => x.ActualHits);
            var minHitsPerGame = playerStats.Min(x => x.ActualHits);
            var averageHitsPerGame = playerStats.Average(x => x.ActualHits);
            var maxHitsPerGame = playerStats.Max(x => x.ActualHits);

            return new AsyncPlayerCombatStatsDto(expectedHits, actualHits, minHitsPerGame, maxHitsPerGame, averageHitsPerGame);
        }

        return new AsyncPlayerCombatStatsDto(0, 0, 0, 0, 0);
    }
}
