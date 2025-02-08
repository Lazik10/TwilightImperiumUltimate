using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncHistoryStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncHistoryStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncHistorySummaryStatsDto> CreateAsyncHistoryStatsSummary(CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);

        var tiglGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var allHistoryStats = CreateHistoryStats(games);
        var tiglHistoryStats = CreateHistoryStats(tiglGames);
        var customHistoryStats = CreateHistoryStats(customGames);

        return new AsyncHistorySummaryStatsDto(allHistoryStats, tiglHistoryStats, customHistoryStats);
    }

    private AsyncHistoryStatsDto CreateHistoryStats(List<GameStats> games)
    {
        var historyStats = CalculateAsyncGamesHistory(games);
        var playersHistoryStats = CalculateAsyncPlayerHistory(games);

        return new AsyncHistoryStatsDto(historyStats, playersHistoryStats);
    }

    private List<AsyncGamesHistoryDto> CalculateAsyncGamesHistory(List<GameStats> games)
    {
        var gameRecords = games
            .Select(game => new
            {
                DateTimeOffset.FromUnixTimeSeconds(game.Timestamp).UtcDateTime.Year,
                DateTimeOffset.FromUnixTimeSeconds(game.Timestamp).UtcDateTime.Month,
            })
            .GroupBy(x => new { x.Year, x.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Count = g.Count(),
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        var cumulativeGames = 0;
        var result = new List<AsyncGamesHistoryDto>();

        foreach (var entry in gameRecords)
        {
            cumulativeGames += entry.Count;

            result.Add(new AsyncGamesHistoryDto(
                entry.Year,
                entry.Month,
                cumulativeGames,
                entry.Count));
        }

        return result;
    }

    private List<AsyncPlayersHistoryDto> CalculateAsyncPlayerHistory(List<GameStats> games)
    {
        var playerRecords = new List<(int Year, int Month, long DiscordUserID)>();

        foreach (var game in games)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(game.Timestamp).UtcDateTime;
            int year = dateTime.Year;
            int month = dateTime.Month;

            foreach (var player in game.PlayerStatistics)
            {
                playerRecords.Add((year, month, player.DiscordUserID));
            }
        }

        var groupedByMonth = playerRecords
            .GroupBy(x => new { x.Year, x.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                UniquePlayers = g.Select(x => x.DiscordUserID).Distinct().ToHashSet(),
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        var cumulativePlayers = new HashSet<long>();
        var result = new List<AsyncPlayersHistoryDto>();

        foreach (var entry in groupedByMonth)
        {
            var newPlayers = entry.UniquePlayers.Except(cumulativePlayers).ToList();
            cumulativePlayers.UnionWith(entry.UniquePlayers);

            result.Add(new AsyncPlayersHistoryDto(
                entry.Year,
                entry.Month,
                cumulativePlayers.Count,
                newPlayers.Count));
        }

        return result;
    }
}
