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
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);
        var playerProfiles = await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken);

        var tiglGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateOpponentsStats(games, playerProfiles, limit);
        var tiglGameStats = CreateOpponentsStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateOpponentsStats(customGames, playerProfiles, limit);

        return new AsyncOpponentsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncOpponentsStatsDto CreateOpponentsStats(List<GameStats> games, List<AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playerGamesMap = games
            .SelectMany(game => game.PlayerStatistics.Select(player => (player.DiscordUserID, game)))
            .GroupBy(x => x.DiscordUserID)
            .ToDictionary(g => g.Key, g => g.Select(x => x.game).ToList());

        var playerDtos = playerGamesMap
            .Select(kvp =>
            {
                var playerInfo = GetPlayerInfo(kvp.Key, playerProfiles);
                var playerId = playerInfo.Id;
                var playerGames = kvp.Value;
                var gamesPlayed = playerGames.Count;

                var uniqueOpponents = playerGames
                    .SelectMany(game => game.PlayerStatistics.Select(p => p.DiscordUserID))
                    .Where(opponentId => opponentId != kvp.Key)
                    .Distinct()
                    .Count();

                return new AsyncOpponentsPlayerDto(
                    playerId,
                    playerInfo.Name,
                    uniqueOpponents,
                    gamesPlayed);
            })
            .OrderByDescending(dto => dto.UniqueOpponents)
            .Take(limit)
            .ToList();

        return new AsyncOpponentsStatsDto(playerDtos);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, List<AsyncPlayerProfile> playerProfiles)
    {
        var player = playerProfiles.Find(x => x.DiscordUserId == discordUserId);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowOpponents && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
