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
        var playerGames = games.Where(x => x.PlayerStatistics.Any(x => x.DiscordUserID == discordUserId)).ToList();
        var tenVpGames = playerGames.Where(x => x.Scoreboard == 10).ToList();
        var twelveVpGames = playerGames.Where(x => x.Scoreboard == 12).ToList();
        var fourteenVpGames = playerGames.Where(x => x.Scoreboard == 14).ToList();

        var playerStats10Vp = tenVpGames.SelectMany(x => x.PlayerStatistics).Where(x => x.DiscordUserID == discordUserId).ToList();
        var playerStats12Vp = twelveVpGames.SelectMany(x => x.PlayerStatistics).Where(x => x.DiscordUserID == discordUserId).ToList();
        var playerStats14Vp = fourteenVpGames.SelectMany(x => x.PlayerStatistics).Where(x => x.DiscordUserID == discordUserId).ToList();

        var tenVpStats = CalculateVpStatsByVpGame(tenVpGames, playerStats10Vp, 10);
        var twelveVpStats = CalculateVpStatsByVpGame(twelveVpGames, playerStats12Vp, 12);
        var fourteenVpStats = CalculateVpStatsByVpGame(fourteenVpGames, playerStats14Vp, 14);

        var totalVp = playerStats10Vp.Sum(x => x.Score) + playerStats12Vp.Sum(x => x.Score) + playerStats14Vp.Sum(x => x.Score);
        var maxPossibleVp = games.Sum(x => x.Scoreboard);
        var averageVpPerGame = games.Count == 0 ? 0 : totalVp / (float)games.Count;
        var weightedVpPercentage = CalculateWeightedVpPercentage(
            tenVpGames.Count,
            AvgVpPercentagePerGame(tenVpGames, playerStats10Vp, 10),
            twelveVpGames.Count,
            AvgVpPercentagePerGame(twelveVpGames, playerStats12Vp, 12),
            fourteenVpGames.Count,
            AvgVpPercentagePerGame(fourteenVpGames, playerStats14Vp, 14));

        var vpStatsTotal = new AsyncPlayerVpStatsByVpGameDto(
            -1,
            totalVp,
            maxPossibleVp,
            averageVpPerGame,
            weightedVpPercentage,
            games.Count);

        return new AsyncPlayerVpStatsDto(new List<AsyncPlayerVpStatsByVpGameDto>() { tenVpStats, twelveVpStats, fourteenVpStats }, vpStatsTotal);
    }

    private static AsyncPlayerVpStatsByVpGameDto CalculateVpStatsByVpGame(List<GameStats> games, List<PlayerStats> playerStats, int vpCategory)
    {
        return new AsyncPlayerVpStatsByVpGameDto(
            vpCategory,
            playerStats.Sum(x => x.Score),
            games.Sum(x => x.Scoreboard),
            playerStats.Count == 0 ? 0 : (float)playerStats.Average(x => x.Score),
            games.Count == 0 ? 0 : playerStats.Sum(x => x.Score) / ((float)games.Count * vpCategory) * 100,
            games.Count);
    }

    private static float CalculateWeightedVpPercentage(int games10, float vpPercentage10, int games12, float vpPercentage12, int games14, float vpPercentage14)
    {
        int totalGames = games10 + games12 + games14;

        if (totalGames == 0) return 0; // Avoid division by zero

        float combinedVp = (vpPercentage10 * games10) +
                           (vpPercentage12 * games12) +
                           (vpPercentage14 * games14);

        return combinedVp / totalGames;
    }

    private static float AvgVpPercentagePerGame(List<GameStats> games, List<PlayerStats> playerStats, int vpCategory)
    {
        return games.Count == 0 ? 0 : playerStats.Sum(x => x.Score) / ((float)games.Count * vpCategory) * 100;
    }
}
