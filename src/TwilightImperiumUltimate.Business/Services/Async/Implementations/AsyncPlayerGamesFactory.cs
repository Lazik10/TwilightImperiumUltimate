using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerGamesFactory : IAsyncPlayerGamesFactory
{
    public Task<AsyncPlayerGamesSummaryDto> CreateAsyncPlayerGames(AsyncPlayerProfile playerProfile)
    {
        var games = playerProfile.GameStatistics.Select(x => x.GameStats).ToList();
        var gamesDto = new List<AsyncPlayerGameDto>();

        foreach (var game in games)
        {
            var playerStats = game.PlayerStatistics.FirstOrDefault(x => x.DiscordUserID == playerProfile.DiscordUserId);

            var isWinner = playerStats is not null && playerStats.Score >= game.Scoreboard;
            var playerScore = playerStats?.Score ?? 0;
            var faction = playerStats?.FactionName ?? AsyncFactionName.Homebrew;

            gamesDto.Add(new AsyncPlayerGameDto(
                game.AsyncGameID,
                game.AsyncFunGameName,
                game.Scoreboard,
                playerScore,
                game.SetupTimestamp,
                game.EndedTimestamp ?? 0,
                game.EndedTimestamp is not null,
                game.HasWinner,
                game.IsTigl,
                game.Round,
                isWinner,
                faction));
        }

        return Task.FromResult(new AsyncPlayerGamesSummaryDto(gamesDto));
    }
}
