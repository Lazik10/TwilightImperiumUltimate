using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerFactionStatsFactory : IAsyncPlayerFactionStatsFactory
{
    public Task<AsyncPlayerFactionStatsSummaryDto> CreateAsyncPlayerFactionStats(AsyncPlayerProfile playerProfile)
    {
        var endedGames = playerProfile.GameStatistics
            .Select(x => x.GameStats)
            .Where(x => x.EndedTimestamp != null && x.HasWinner)
            .ToList();

        var endedTiglGames = endedGames.Where(x => x.IsTigl).ToList();
        var endedCustomGames = endedGames.Where(x => !x.IsTigl).ToList();

        var overallFactionStats = CreateFactionStatsForPlayer(endedGames, playerProfile.DiscordUserId);
        var tiglFactionStats = CreateFactionStatsForPlayer(endedTiglGames, playerProfile.DiscordUserId);
        var customFactionStats = CreateFactionStatsForPlayer(endedCustomGames, playerProfile.DiscordUserId);

        return Task.FromResult(new AsyncPlayerFactionStatsSummaryDto(overallFactionStats, tiglFactionStats, customFactionStats));
    }

    private static List<AsyncPlayerFactionStatsDto> CreateFactionStatsForPlayer(List<GameStats> games, long discordUserId)
    {
        var factionStatsDict = CreateAsyncFactionsDictionary();

        foreach (var game in games)
        {
            var allPlayerStats = game.PlayerStatistics
                .Where(x => x.DiscordUserID == discordUserId)
                .ToList();

            foreach (var playerStats in allPlayerStats)
            {
                var faction = playerStats.FactionName;
                var factionStats = factionStatsDict[faction];

                UpdateFactionByVp(game.Scoreboard, factionStats, playerStats);
                UpdateOverallFactionStats(game.Scoreboard, factionStats.All, playerStats);
            }
        }

        return factionStatsDict.Values.ToList();
    }

    private static void UpdateOverallFactionStats(int scoreboard, AsyncPlayerFactionStatsByGameVp factionStats, PlayerStats playerStats)
    {
        factionStats.Games++;
        factionStats.Wins += playerStats.Winner ? 1 : 0;
        factionStats.Eliminations += playerStats.Eliminated ? 1 : 0;
        factionStats.MaxPossibleVp += scoreboard;
        factionStats.TotalScoredVp += playerStats.Score;
        if (factionStats.MinVp == 0 && playerStats.Score != 0)
            factionStats.MinVp = playerStats.Score;
        else if (factionStats.MinVp != 0 && playerStats.Score != 0 && playerStats.Score < factionStats.MinVp)
            factionStats.MinVp = playerStats.Score;
        factionStats.MaxVp = Math.Max(factionStats.MaxVp, playerStats.Score);
    }

    private static void UpdateFactionByVp(int scoreboard, AsyncPlayerFactionStatsDto factionStats, PlayerStats playerStats)
    {
        var correctfactionStats = scoreboard switch
        {
            10 => factionStats.TenVp,
            12 => factionStats.TwelveVp,
            14 => factionStats.FourteenVp,
            _ => null,
        };

        if (correctfactionStats is not null)
        {
            correctfactionStats.Games++;
            correctfactionStats.Wins += playerStats.Winner ? 1 : 0;
            correctfactionStats.Eliminations += playerStats.Eliminated ? 1 : 0;
            correctfactionStats.MaxPossibleVp += scoreboard;
            correctfactionStats.TotalScoredVp += playerStats.Score;
            if (correctfactionStats.MinVp == 0 && playerStats.Score != 0)
                correctfactionStats.MinVp = playerStats.Score;
            else if (correctfactionStats.MinVp != 0 && playerStats.Score != 0 && playerStats.Score < correctfactionStats.MinVp)
                correctfactionStats.MinVp = playerStats.Score;
            correctfactionStats.MaxVp = Math.Max(correctfactionStats.MaxVp, playerStats.Score);
        }
    }

    private static Dictionary<AsyncFactionName, AsyncPlayerFactionStatsDto> CreateAsyncFactionsDictionary()
    {
        var factionStatsDict = new Dictionary<AsyncFactionName, AsyncPlayerFactionStatsDto>();

        foreach (AsyncFactionName faction in Enum.GetValues(typeof(AsyncFactionName)))
        {
            factionStatsDict[faction] = new AsyncPlayerFactionStatsDto() { FactionName = faction };
        }

        return factionStatsDict;
    }
}
