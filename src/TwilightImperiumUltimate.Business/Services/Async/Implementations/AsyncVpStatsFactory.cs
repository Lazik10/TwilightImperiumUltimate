using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncVpStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncVpStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncVpSummaryStatsDto> CreateAsyncVpStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var playerProfiles = await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken);

        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);
        var allFinishedGames = games.Where(x => x.EndedTimestamp != null && x.HasWinner).ToList();
        var tiglGames = allFinishedGames.Where(x => x.IsTigl).ToList();
        var customGames = allFinishedGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateVpStats(allFinishedGames, playerProfiles, limit);
        var tiglGameStats = CreateVpStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateVpStats(customGames, playerProfiles, limit);

        return new AsyncVpSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncVpStatsDto CreateVpStats(List<GameStats> games, List<AsyncPlayerProfile> playerProfiles, int limit)
    {
        var players = games
            .SelectMany(x => x.PlayerStatistics)
            .GroupBy(x => x.DiscordUserID)
            .Where(x => x.Count() >= 20)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncVpPlayerDto(
                    playerInfo.Id,
                    playerInfo.Name,
                    g.Sum(x => x.Score),
                    games
                        .Where(gs => gs.PlayerStatistics.Any(ps => ps.DiscordUserID == g.Key))
                        .Sum(x => x.Scoreboard));
            })
            .ToList();

        var playersWithMostVpPercentage = players
            .OrderByDescending(x => x.VpPercentage)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var playersWithMostVp = players
            .OrderByDescending(x => x.Vp)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        return new AsyncVpStatsDto(playersWithMostVpPercentage, playersWithMostVp);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, List<AsyncPlayerProfile> playerProfiles)
    {
        var player = playerProfiles.Find(x => x.DiscordUserId == discordUserId);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowVpStats && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
