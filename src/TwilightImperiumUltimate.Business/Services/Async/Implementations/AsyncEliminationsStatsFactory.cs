using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncEliminationsStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncEliminationsStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncEliminationsSummaryStatsDto> CreateAsyncEliminationsStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);
        var playerProfiles = await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken);

        var allGames = games
            .Where(x => x.EndedTimestamp != null && x.HasWinner)
            .ToList();
        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateEliminationsStats(allGames, playerProfiles, limit);
        var tiglGameStats = CreateEliminationsStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateEliminationsStats(customGames, playerProfiles, limit);

        return new AsyncEliminationsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncEliminationsStatsDto CreateEliminationsStats(List<GameStats> games, List<AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playerDtos = games
            .SelectMany(x => x.PlayerStatistics)
            .GroupBy(x => x.DiscordUserID)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncEliminationsPlayerDto(
                playerInfo.Id,
                playerInfo.Name,
                g.Count(x => x.Eliminated),
                g.Count());
            })
            .Where(x => x.Games >= 20)
            .ToList();

        var mostEliminationsPercentagePlayers = playerDtos
            .OrderByDescending(x => x.EliminationsPercentage)
            .ThenBy(x => x.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var mostEliminationsPlayers = playerDtos
            .OrderByDescending(x => x.Eliminations)
            .ThenBy(x => x.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        return new AsyncEliminationsStatsDto(mostEliminationsPlayers, mostEliminationsPercentagePlayers);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, List<AsyncPlayerProfile> playerProfiles)
    {
        var player = playerProfiles.Find(x => x.DiscordUserId == discordUserId);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowGames && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
