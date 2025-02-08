using System.Xml.Schema;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncDurationsStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncDurationsStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncDurationsSummaryStatsDto> CreateAsyncDurationsStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);

        var allGames = games
            .Where(x => x.EndedTimestamp != null
                && x.HasWinner
                && x.EndedTimestamp != 0
                && x.PlayerStatistics.Any(y => y.Score >= x.Scoreboard))
            .ToList();

        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateDurationsStats(allGames, limit);
        var tiglGameStats = CreateDurationsStats(tiglGames, limit);
        var customGameStats = CreateDurationsStats(customGames, limit);

        return new AsyncDurationsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncDurationsStatsDto CreateDurationsStats(List<GameStats> games, int limit)
    {
        var longestGames = games
            .Where(x => x.NumberOfPlayers >= 6)
            .Select(x => new AsyncDurationDto(
                x.AsyncGameID,
                x.AsyncFunGameName,
                (x.EndedTimestamp ?? 0) - x.SetupTimestamp,
                x.Scoreboard,
                x.NumberOfPlayers))
            .OrderByDescending(x => x.Duration)
            .Take(limit)
            .ToList();

        var fastestGames = games
            .Where(x => x.NumberOfPlayers >= 6)
            .Select(x => new AsyncDurationDto(
                x.AsyncGameID,
                x.AsyncFunGameName,
                (x.EndedTimestamp ?? 0) - x.SetupTimestamp,
                x.Scoreboard,
                x.NumberOfPlayers))
            .OrderBy(x => x.Duration)
            .Take(limit)
            .ToList();

        return new AsyncDurationsStatsDto(
            longestGames,
            fastestGames);
    }
}
