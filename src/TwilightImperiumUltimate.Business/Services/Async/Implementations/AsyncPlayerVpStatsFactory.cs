using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerVpStatsFactory : IAsyncPlayerVpStatsFactory
{
    public Task<AsyncPlayerVpStatsSummaryDto> CreateAsyncPlayerVpStats(AsyncPlayerProfile playerProfile)
    {
        var games = playerProfile.GameStatistics
            .Select(x => x.GameStats)
            .Where(x => x.EndedTimestamp != null && x.HasWinner)
            .ToList();

        var tigerGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var vpStatsSummary = new AsyncPlayerVpStatsSummaryDto();

        vpStatsSummary.All = CreateVpStatsForPlayer(games, playerProfile.DiscordUserId);
        vpStatsSummary.Tigl = CreateVpStatsForPlayer(tigerGames, playerProfile.DiscordUserId);
        vpStatsSummary.Custom = CreateVpStatsForPlayer(customGames, playerProfile.DiscordUserId);

        return Task.FromResult(vpStatsSummary);
    }

    private static AsyncPlayerVpStatsDto CreateVpStatsForPlayer(List<GameStats> games, long discordUserId)
    {
        return new AsyncPlayerVpStatsDto(
            GetAverageGameScore(games, 10, discordUserId),
            GetGamesCount(games, 10, discordUserId),
            GetAverageGameScore(games, 12, discordUserId),
            GetGamesCount(games, 12, discordUserId),
            GetAverageGameScore(games, 14, discordUserId),
            GetGamesCount(games, 14, discordUserId));
    }

    private static float GetAverageGameScore(List<GameStats> games, int victoryPoints, long discordId)
    {
        var gamesWithCorrectVp = games.Where(x => x.Scoreboard == victoryPoints).ToList();
        if (gamesWithCorrectVp.Count == 0)
            return 0;

        var statsWithCorrectVp = gamesWithCorrectVp
            .SelectMany(x => x.PlayerStatistics.Where(x => x.DiscordUserID == discordId))
            .ToList();

        if (statsWithCorrectVp.Count == 0)
            return 0;

        return (float)statsWithCorrectVp
            .Average(x => x.Score);
    }

    private static int GetGamesCount(List<GameStats> games, int victoryPoints, long discordId)
    {
        var gamesWithCorrectVp = games.Where(x => x.Scoreboard == victoryPoints).ToList();
        if (gamesWithCorrectVp.Count == 0)
            return 0;

        return gamesWithCorrectVp.Count(x => x.PlayerStatistics.Any(x => x.DiscordUserID == discordId));
    }
}
