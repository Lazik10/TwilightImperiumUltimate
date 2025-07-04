using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerGamesFactory : IAsyncPlayerGamesFactory
{
    private const string FogOfWar = "fow";

    public Task<AsyncPlayerGamesSummaryDto> CreateAsyncPlayerGames(AsyncPlayerProfile playerProfile)
    {
        var games = playerProfile.GameStatistics.Select(x => x.GameStats).ToList();
        var gamesDto = new List<AsyncPlayerGameDto>();

        foreach (var game in games)
        {
            int playerScore = 0;
            bool isWinner = false;
            AsyncFactionName faction = AsyncFactionName.Unknown;

            var playerStats = game.PlayerStatistics.FirstOrDefault(x => x.DiscordUserID == playerProfile.DiscordUserId);
            var isActiveFowGame = !game.HasWinner && game.EndedTimestamp is null && game.AsyncGameID.StartsWith(FogOfWar);
            var asyncGameId = isActiveFowGame ? FogOfWar : game.AsyncGameID;

            if (playerStats is not null)
            {
                isWinner = playerStats.Score >= game.Scoreboard;
                playerScore = isActiveFowGame ? 0 : playerStats.Score;
                faction = isActiveFowGame ? AsyncFactionName.Unknown : playerStats.FactionName;
            }

            gamesDto.Add(new AsyncPlayerGameDto(
                asyncGameId,
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
