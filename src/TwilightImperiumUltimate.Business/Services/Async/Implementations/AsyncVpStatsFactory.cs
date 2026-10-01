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
        var playerProfiles = (await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken))
            .ToDictionary(x => x.DiscordUserId);

        var games = await _asyncStatsRepository.GetAsyncStatisticsGameProjections(cancellationToken);
        var allFinishedGames = games.Where(x => x.EndedTimestamp != null && x.HasWinner).ToList();
        var tiglGames = allFinishedGames.Where(x => x.IsTigl).ToList();
        var customGames = allFinishedGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateVpStats(allFinishedGames, playerProfiles, limit);
        var tiglGameStats = CreateVpStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateVpStats(customGames, playerProfiles, limit);

        return new AsyncVpSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncVpStatsDto CreateVpStats(List<AsyncStatisticsGameProjection> games, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles, int limit)
    {
        var players = games
            .Where(x => x.DiscordUserId.HasValue)
            .GroupBy(x => x.DiscordUserId!.Value)
            .Where(x => x.Count() >= 20)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncVpPlayerDto(
                    playerInfo.Id,
                    playerInfo.Name,
                    g.Sum(x => x.Score ?? 0),
                    games
                        .Where(gs => gs.DiscordUserId == g.Key)
                        .GroupBy(x => x.GameStatsId)
                        .Sum(x => x.First().Scoreboard),
                    g.Select(x => x.GameStatsId).Distinct().Count());
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

    private (int Id, string Name) GetPlayerInfo(long discordUserId, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles)
    {
        playerProfiles.TryGetValue(discordUserId, out var player);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowVpStats && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
