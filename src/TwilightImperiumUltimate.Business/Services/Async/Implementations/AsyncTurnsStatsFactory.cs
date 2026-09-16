using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncTurnsStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncTurnsStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncTurnsSummaryStatsDto> CreateAsyncTurnsStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var allGames = await _asyncStatsRepository.GetAsyncStatisticsGameProjections(cancellationToken);
        var playerProfiles = (await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken))
            .ToDictionary(x => x.DiscordUserId);

        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateTurnsStats(allGames, playerProfiles, limit);
        var tiglGameStats = CreateTurnsStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateTurnsStats(customGames, playerProfiles, limit);

        return new AsyncTurnsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncTurnsStatsDto CreateTurnsStats(List<AsyncStatisticsGameProjection> games, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playerDtos = games
            .Where(x => x.EndedTimestamp != null && x.HasWinner)
            .Where(x => x.DiscordUserId.HasValue)
            .GroupBy(x => x.DiscordUserId!.Value)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncTurnsPlayerDto(
                playerInfo.Id,
                playerInfo.Name,
                g.Sum(x => x.TotalNumberOfTurns ?? 0),
                g.Sum(x => x.TotalTurnTime ?? 0),
                g.Select(x => x.GameStatsId).Distinct().Count());
            })
            .Where(x => x.Turns > 500)
            .ToList();

        var playersWithMostTurns = playerDtos
            .OrderByDescending(x => x.Turns)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var playersWithLowestAverageTurnTime = playerDtos
            .OrderBy(x => x.AverageTurnTime)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        return new AsyncTurnsStatsDto(playersWithLowestAverageTurnTime, playersWithMostTurns);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles)
    {
        playerProfiles.TryGetValue(discordUserId, out var player);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowTurnStats && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
