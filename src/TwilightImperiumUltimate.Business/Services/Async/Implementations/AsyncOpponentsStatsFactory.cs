using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncOpponentsStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncOpponentsStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncOpponentsSummaryStatsDto> CreateAsyncOpponentsStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAsyncStatisticsGameProjections(cancellationToken);
        var playerProfiles = (await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken))
            .ToDictionary(x => x.DiscordUserId);

        var tiglGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateOpponentsStats(games, playerProfiles, limit);
        var tiglGameStats = CreateOpponentsStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateOpponentsStats(customGames, playerProfiles, limit);

        return new AsyncOpponentsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncOpponentsStatsDto CreateOpponentsStats(List<AsyncStatisticsGameProjection> games, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playersByGame = games
            .GroupBy(x => x.GameStatsId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Where(x => x.DiscordUserId.HasValue)
                    .Select(x => x.DiscordUserId!.Value)
                    .Distinct()
                    .ToList());

        var playerGamesMap = games
            .Where(x => x.DiscordUserId.HasValue)
            .GroupBy(x => x.DiscordUserId!.Value)
            .ToDictionary(g => g.Key, g => g.Select(x => x.GameStatsId).Distinct().ToList());

        var playerDtos = playerGamesMap
            .Select(kvp =>
            {
                var playerInfo = GetPlayerInfo(kvp.Key, playerProfiles);
                var playerId = playerInfo.Id;
                var playerGameIds = kvp.Value;
                var gamesPlayed = playerGameIds.Count;

                var uniqueOpponents = playerGameIds
                    .SelectMany(gameId => playersByGame[gameId])
                    .Where(opponentId => opponentId != kvp.Key)
                    .Distinct()
                    .Count();

                return new AsyncOpponentsPlayerDto(
                    playerId,
                    playerInfo.Name,
                    uniqueOpponents,
                    gamesPlayed);
            })
            .Where(dto => dto.UniqueOpponents > 0)
            .OrderByDescending(dto => dto.UniqueOpponents)
            .Take(limit)
            .ToList();

        return new AsyncOpponentsStatsDto(playerDtos);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, IReadOnlyDictionary<long, AsyncPlayerProfile> playerProfiles)
    {
        playerProfiles.TryGetValue(discordUserId, out var player);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowOpponents && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
