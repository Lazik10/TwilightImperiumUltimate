using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncGamesStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncGamesStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncGamesSummaryStatsDto> CreateAsyncGamesStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var allGames = await _asyncStatsRepository.GetAsyncStatisticsGameProjections(cancellationToken);
        var playerProfiles = (await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken))
            .ToDictionary(x => x.DiscordUserId);

        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateGameStats(allGames, playerProfiles, limit);
        var tiglGameStats = CreateGameStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateGameStats(customGames, playerProfiles, limit);

        return new AsyncGamesSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncGamesStatsDto CreateGameStats(List<AsyncStatisticsGameProjection> games, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playersWithMostGames = games
            .Where(x => x.EndedTimestamp != null && x.HasWinner)
                .Where(x => x.DiscordUserId.HasValue)
                .GroupBy(x => x.DiscordUserId!.Value)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncGamesPlayerDto(
                    playerInfo.Id,
                    playerInfo.Name,
                    g.Count(),
                    0);
            })
            .OrderByDescending(g => g.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var activeGames = games.Where(x => x.EndedTimestamp == null).ToList();

        var playersWithMostActiveGames = activeGames
            .Where(x => x.DiscordUserId.HasValue)
                .GroupBy(x => x.DiscordUserId!.Value)
            .Select(g =>
             {
                 var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                 return new AsyncGamesPlayerDto(
                     playerInfo.Id,
                     playerInfo.Name,
                     0,
                     g.Count());
             })
            .OrderByDescending(g => g.Active)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        return new AsyncGamesStatsDto(playersWithMostGames, playersWithMostActiveGames);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles)
    {
        playerProfiles.TryGetValue(discordUserId, out var player);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowGames && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
