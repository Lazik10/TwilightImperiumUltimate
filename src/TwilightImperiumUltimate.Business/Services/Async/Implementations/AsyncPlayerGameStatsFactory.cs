using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.MainStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerGameStatsFactory : IAsyncPlayerGameStatsFactory
{
    public Task<AsyncPlayerMainStatsSummaryDto> CreateAsyncPlayerGameStats(AsyncPlayerProfile playerProfile)
    {
        var games = playerProfile.GameStatistics
            .Select(x => x.GameStats)
            .Where(x => (x.EndedTimestamp != null && x.HasWinner) || x.EndedTimestamp == null)
            .ToList();

        var tiglGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var overallGameStats = GetGameStats(games, playerProfile.DiscordUserId);
        var tiglGameStats = GetGameStats(tiglGames, playerProfile.DiscordUserId);
        var customGameStats = GetGameStats(customGames, playerProfile.DiscordUserId);

        return Task.FromResult(new AsyncPlayerMainStatsSummaryDto(overallGameStats, tiglGameStats, customGameStats));
    }

    private static AsyncPlayerMainStatsDto GetGameStats(List<GameStats> gameStats, long discordUserId)
    {
        var gamesCount = gameStats.Count;
        var wins = gameStats.Count(x => x.PlayerStatistics.Any(y => y.DiscordUserID == discordUserId && y.Winner));
        var eliminations = gameStats.Count(x => x.PlayerStatistics.Any(y => y.DiscordUserID == discordUserId && y.Eliminated));
        var active = gameStats.Count(x => x.EndedTimestamp == null);
        var finished = gamesCount - active;

        return new AsyncPlayerMainStatsDto(gamesCount, wins, eliminations, active, finished);
    }
}
