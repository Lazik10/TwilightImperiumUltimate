using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncFactionStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncFactionStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncFactionsSummaryStatsDto> CreateAsyncFactionStatsSummary(CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);

        var allGames = games
            .Where(x =>
                x.EndedTimestamp != null
                && !x.FrankenGame)
            .ToList();

        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateFactionStats(allGames);
        var tiglGameStats = CreateFactionStats(tiglGames);
        var customGameStats = CreateFactionStats(customGames);

        return new AsyncFactionsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private static List<AsyncFactionsStatsDto> CreateFactionStats(List<GameStats> games)
    {
        var factionStatsDict = CreateAsyncFactionsDictionary();

        foreach (var game in games)
        {
            var allPlayerStats = game.PlayerStatistics.ToList();

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

    private static void UpdateOverallFactionStats(int scoreboard, AsyncFactionStatsByGameVpDto factionStats, PlayerStats playerStats)
    {
        factionStats.Games++;
        factionStats.Wins += playerStats.Winner ? 1 : 0;
        factionStats.Eliminations += playerStats.Eliminated ? 1 : 0;
        factionStats.Vp += playerStats.Score;
        factionStats.MaxVp += scoreboard;
    }

    private static void UpdateFactionByVp(int scoreboard, AsyncFactionsStatsDto factionStats, PlayerStats playerStats)
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
            correctfactionStats.MaxVp += scoreboard;
            correctfactionStats.Vp += playerStats.Score;
        }
    }

    private static Dictionary<AsyncFactionName, AsyncFactionsStatsDto> CreateAsyncFactionsDictionary()
    {
        var factionStatsDict = new Dictionary<AsyncFactionName, AsyncFactionsStatsDto>();

        foreach (AsyncFactionName faction in Enum.GetValues<AsyncFactionName>())
        {
            factionStatsDict[faction] = new AsyncFactionsStatsDto() { FactionName = faction };
        }

        return factionStatsDict;
    }
}
