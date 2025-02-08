using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerTurnStatsFactory : IAsyncPlayerTurnStatsFactory
{
    public Task<AsyncPlayerTurnStatsSummaryDto> CreateAsyncPlayerTurnStats(AsyncPlayerProfile playerProfile)
    {
        var games = playerProfile.GameStatistics.Select(x => x.GameStats).ToList();
        var tiglGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var totalTurnStats = GetTurnStats(games, playerProfile.DiscordUserId);
        var tiglTurnStats = GetTurnStats(tiglGames, playerProfile.DiscordUserId);
        var customTurnStats = GetTurnStats(customGames, playerProfile.DiscordUserId);

        return Task.FromResult(new AsyncPlayerTurnStatsSummaryDto(totalTurnStats, tiglTurnStats, customTurnStats));
    }

    private static AsyncPlayerTurnStatsDto GetTurnStats(List<GameStats> gameStats, long discordUserId)
    {
        var totalTurns = gameStats
            .SelectMany(x => x.PlayerStatistics)
            .Where(x => x.DiscordUserID == discordUserId)
            .Sum(x => x.TotalNumberOfTurns);

        var totalTurnTime = gameStats
            .SelectMany(x => x.PlayerStatistics)
            .Where(x => x.DiscordUserID == discordUserId)
            .Sum(x => x.TotalTurnTime);

        return new AsyncPlayerTurnStatsDto(totalTurns, totalTurnTime);
    }
}
