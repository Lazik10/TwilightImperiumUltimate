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
        var allGames = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);
        var playerProfiles = await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken);

        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateTurnsStats(allGames, playerProfiles, limit);
        var tiglGameStats = CreateTurnsStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateTurnsStats(customGames, playerProfiles, limit);

        return new AsyncTurnsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncTurnsStatsDto CreateTurnsStats(List<GameStats> games, List<AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playerDtos = games
            .Where(x => x.EndedTimestamp != null && x.HasWinner)
            .SelectMany(x => x.PlayerStatistics)
            .GroupBy(x => x.DiscordUserID)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncTurnsPlayerDto(
                playerInfo.Id,
                playerInfo.Name,
                g.Sum(x => x.TotalNumberOfTurns),
                g.Sum(x => x.TotalTurnTime));
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

    private (int Id, string Name) GetPlayerInfo(long discordUserId, List<AsyncPlayerProfile> playerProfiles)
    {
        var player = playerProfiles.Find(x => x.DiscordUserId == discordUserId);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowTurnStats && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
