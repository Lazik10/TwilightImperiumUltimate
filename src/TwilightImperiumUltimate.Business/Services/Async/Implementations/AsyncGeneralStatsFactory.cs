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
        var allGameRows = await _asyncStatsRepository.GetAsyncGeneralStatsGameProjections(cancellationToken);
        var allGames = GroupGames(allGameRows);
        var tiglGames = allGames.Where(x => x.Details.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.Details.IsTigl).ToList();

        var allGameStats = CreateGameStats(allGames);
        var tiglGameStats = CreateGameStats(tiglGames);
        var customGameStats = CreateGameStats(customGames);

        return new AsyncGeneralSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncGeneralStatsDto CreateGameStats(IReadOnlyList<GeneralStatsGame> games)
    {
        var gamesCount = games.Count;
        var activeGames = games.Count(g => g.Details.EndedTimestamp == null);
        var cancelledGames = games.Count(g => g.Details.EndedTimestamp is not null && !g.Details.HasWinner);
        var finishedGames = gamesCount - activeGames - cancelledGames;
        var eliminations = games.Count(g => g.Players.Any(ps => ps.Eliminated == true));

        var players = games.SelectMany(g => g.Players)
            .Where(x => x.DiscordUserId.HasValue)
            .Select(x => x.DiscordUserId!.Value)
            .Distinct()
            .Count();

        var activePlayers = games.Where(g => g.Details.EndedTimestamp == null)
            .SelectMany(g => g.Players)
            .Where(x => x.DiscordUserId.HasValue)
            .Select(x => x.DiscordUserId!.Value)
            .Distinct()
            .ToHashSet();

        var inactiveGames = games
            .Where(g => g.Details.EndedTimestamp is not null)
            .ToList();

        var inactivePlayers = inactiveGames
            .SelectMany(g => g.Players)
            .Where(x => x.DiscordUserId.HasValue)
            .Select(x => x.DiscordUserId!.Value)
            .Where(x => !activePlayers.Contains(x))
            .Distinct()
            .ToHashSet();

        var inactiveLessThanThreeMonths = inactiveGames
            .Where(g => IsLessThanMonthsInactive(g.Details, 3))
            .SelectMany(g => g.Players)
            .Where(x => x.DiscordUserId.HasValue)
            .Select(x => x.DiscordUserId!.Value)
            .Where(inactivePlayers.Contains)
            .Distinct()
            .ToList();

        var inactiveMoreThanThreeMonths = games
            .Where(g => IsMoreThanMonthsInactive(g.Details, 3))
            .SelectMany(g => g.Players)
            .Where(x => x.DiscordUserId.HasValue)
            .Select(x => x.DiscordUserId!.Value)
            .Where(x => inactivePlayers.Contains(x) && !inactiveLessThanThreeMonths.Contains(x))
            .Distinct()
            .Count();

        var distributionsByPlayerTime = games
            .SelectMany(game => game.Players)
            .Where(stat => stat.DiscordUserId.HasValue)
            .GroupBy(stat => stat.DiscordUserId!.Value)
            .Select(group =>
            {
                int totalTurns = group.Sum(stat => stat.TotalNumberOfTurns ?? 0);
                double averageTimeHours = group.Sum(stat => stat.TotalTurnTime ?? 0) / (totalTurns * 3600000.0);

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
            .GroupBy(x => GetVpDistributionKey(x.Details))
            .Select(x => new GameDistributionByVpDto(x.Key, x.Count()))
            .ToList();

        var distributionByPlayerCount = games
            .GroupBy(x => GetPlayerCountDistributionKey(x.Details))
            .Select(x => new GameDistributionByPlayerCountDto(x.Key, x.Count()))
            .ToList();

        var distributionByAverageTurnEnd = games
            .GroupBy(x => GetVpDistributionKey(x.Details))
            .Select(x => new GameDitributionByAverageTurnEndDto(x.Key, x.Average(game => game.Details.Round)))
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

    private List<GeneralStatsGame> GroupGames(IReadOnlyList<AsyncGeneralStatsGameProjection> gameRows)
    {
        return gameRows
            .GroupBy(x => x.GameStatsId)
            .Select(group => new GeneralStatsGame(group.First(), group.Where(x => x.DiscordUserId.HasValue).ToList()))
            .ToList();
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

    private int GetVpDistributionKey(AsyncGeneralStatsGameProjection game)
    {
        return game.Scoreboard switch
        {
            10 => 10,
            12 => 12,
            14 => 14,
            _ => -1,
        };
    }

    private int GetPlayerCountDistributionKey(AsyncGeneralStatsGameProjection game)
    {
        return game.NumberOfPlayers switch
        {
            >= 2 and <= 8 => game.NumberOfPlayers,
            _ => -1,
        };
    }

    private bool IsLessThanMonthsInactive(AsyncGeneralStatsGameProjection game, int months)
    {
        return DateTimeOffset.FromUnixTimeSeconds(game.SetupTimestamp) > DateTimeOffset.UtcNow.AddMonths(-months);
    }

    private bool IsMoreThanMonthsInactive(AsyncGeneralStatsGameProjection game, int months)
    {
        return DateTimeOffset.FromUnixTimeSeconds(game.SetupTimestamp) < DateTimeOffset.UtcNow.AddMonths(-months);
    }

    private sealed record GeneralStatsGame(
        AsyncGeneralStatsGameProjection Details,
        IReadOnlyList<AsyncGeneralStatsGameProjection> Players);
}
