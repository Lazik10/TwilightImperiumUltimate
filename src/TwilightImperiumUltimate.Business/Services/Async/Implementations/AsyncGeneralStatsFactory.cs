using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncGeneralStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncGeneralStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncGeneralSummaryStatsDto> CreateAsyncGeneralStatsSummary(CancellationToken cancellationToken)
    {
        var allGames = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);
        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateGameStats(allGames);
        var tiglGameStats = CreateGameStats(tiglGames);
        var customGameStats = CreateGameStats(customGames);

        return new AsyncGeneralSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncGeneralStatsDto CreateGameStats(List<GameStats> games)
    {
        var gamesCount = games.Count;
        var activeGames = games.Count(g => g.EndedTimestamp == null);
        var cancelledGames = games.Count(g => g.EndedTimestamp is not null && !g.HasWinner);
        var finishedGames = gamesCount - activeGames - cancelledGames;
        var eliminations = games.Count(g => g.PlayerStatistics.Any(ps => ps.Eliminated));

        var players = games.SelectMany(g => g.PlayerStatistics)
            .Select(x => x.DiscordUserID)
            .Distinct()
            .Count();

        var activePlayers = games.Where(g => g.EndedTimestamp == null)
            .SelectMany(g => g.PlayerStatistics)
            .Select(x => x.DiscordUserID)
            .Distinct()
            .ToHashSet();

        var inactiveGames = games
            .Where(g => g.EndedTimestamp is not null)
            .ToList();

        var inactivePlayers = inactiveGames
            .SelectMany(g => g.PlayerStatistics)
            .Select(x => x.DiscordUserID)
            .Where(x => !activePlayers.Contains(x))
            .Distinct()
            .ToHashSet();

        var inactiveLessThanThreeMonths = inactiveGames
            .Where(g => IsLessThanMonthsInactive(g, 3))
            .SelectMany(g => g.PlayerStatistics)
            .Select(x => x.DiscordUserID)
            .Where(inactivePlayers.Contains)
            .Distinct()
            .ToList();

        var inactiveMoreThanThreeMonths = games
            .Where(g => IsMoreThanMonthsInactive(g, 3))
            .SelectMany(g => g.PlayerStatistics)
            .Select(x => x.DiscordUserID)
            .Where(x => inactivePlayers.Contains(x) && !inactiveLessThanThreeMonths.Contains(x))
            .Distinct()
            .Count();

        var distributionsByPlayerTime = games
            .SelectMany(game => game.PlayerStatistics)
            .GroupBy(stat => stat.DiscordUserID)
            .Select(group =>
            {
                int totalTurns = group.Sum(stat => stat.TotalNumberOfTurns);
                double averageTimeHours = group.Sum(stat => stat.TotalTurnTime) / (totalTurns * 3600000.0);

                return new
                {
                    Id = group.Key,
                    Turns = totalTurns,
                    AverageTimeHours = averageTimeHours,
                };
            })
            .Where(player => player.Turns > 100)
            .GroupBy(player => CategorizeTime(player.AverageTimeHours))
            .Select(group => new PlayerDistributionByTimerDto(group.Key, group.Count()))
            .OrderBy(dist => dist.Timer)
            .ToList();

        var distributionByVp = games
            .GroupBy(GetVpDistributionKey)
            .Select(x => new GameDistributionByVpDto(x.Key, x.Count()))
            .ToList();

        var distributionByPlayerCount = games
            .GroupBy(GetPlayerCountDistributionKey)
            .Select(x => new GameDistributionByPlayerCountDto(x.Key, x.Count()))
            .ToList();

        var distributionByAverageTurnEnd = games
            .GroupBy(GetVpDistributionKey)
            .Select(x => new GameDitributionByAverageTurnEndDto(x.Key, x.Average(x => x.Round)))
            .ToList();

        return new AsyncGeneralStatsDto(
            gamesCount,
            activeGames,
            cancelledGames,
            finishedGames,
            eliminations,
            players,
            activePlayers.Count,
            inactivePlayers.Count,
            inactiveLessThanThreeMonths.Count,
            inactiveMoreThanThreeMonths,
            distributionByVp,
            distributionsByPlayerTime,
            distributionByPlayerCount,
            distributionByAverageTurnEnd);
    }

    private int CategorizeTime(double avgTimeHours)
    {
        if (avgTimeHours >= 8) return 9; // 8+ hours grouped together
        if (avgTimeHours >= 7) return 8;
        if (avgTimeHours >= 6) return 7;
        if (avgTimeHours >= 5) return 6;
        if (avgTimeHours >= 4) return 5;
        if (avgTimeHours >= 3) return 4;
        if (avgTimeHours >= 2) return 3;
        if (avgTimeHours >= 1) return 2;

        // Less than 1 hour, split at 30 minutes
        return avgTimeHours >= 0.5 ? 1 : 0;
    }

    private int GetVpDistributionKey(GameStats game)
    {
        return game.Scoreboard switch
        {
            10 => 10,
            12 => 12,
            14 => 14,
            _ => -1,
        };
    }

    private int GetPlayerCountDistributionKey(GameStats game)
    {
        return game.NumberOfPlayers switch
        {
            >= 2 and <= 8 => game.NumberOfPlayers,
            _ => -1,
        };
    }

    private bool IsLessThanMonthsInactive(GameStats game, int months)
    {
        return DateTimeOffset.FromUnixTimeSeconds(game.SetupTimestamp) > DateTimeOffset.UtcNow.AddMonths(-months);
    }

    private bool IsMoreThanMonthsInactive(GameStats game, int months)
    {
        return DateTimeOffset.FromUnixTimeSeconds(game.SetupTimestamp) < DateTimeOffset.UtcNow.AddMonths(-months);
    }
}
