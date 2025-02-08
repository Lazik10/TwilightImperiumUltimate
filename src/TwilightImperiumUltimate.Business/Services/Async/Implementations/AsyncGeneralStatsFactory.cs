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

        var playersByTime = games
            .Where(x => x.EndedTimestamp != null && x.HasWinner)
            .SelectMany(x => x.PlayerStatistics)
            .GroupBy(x => x.DiscordUserID)
            .Select(g =>
            {
                return new
                {
                    Id = g.Key,
                    Turns = g.Sum(x => x.TotalNumberOfTurns),
                    AverageTime = ((float)g.Sum(x => x.TotalTurnTime)) / g.Sum(x => x.TotalNumberOfTurns),
                };
            })
            .Where(x => x.Turns > 100)
            .GroupBy(x => ConvertToTime(x.AverageTime))
            .ToList();

        var distributionsByPlayerTime = playersByTime
            .Select(x => new PlayerDistributionByTimerDto(x.Key, x.Count()))
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

    private int ConvertToTime(float averageTime)
    {
        TimeSpan averageTurnTime = TimeSpan.FromMilliseconds(averageTime);

        return averageTurnTime.Hours switch
        {
            > 8 => 9,
            > 7 => 8,
            > 6 => 7,
            > 5 => 6,
            > 4 => 5,
            > 3 => 4,
            > 2 => 3,
            > 1 => 2,
            > 0 when averageTurnTime.Minutes > 30 => 1,
            _ => 0,
        };
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
