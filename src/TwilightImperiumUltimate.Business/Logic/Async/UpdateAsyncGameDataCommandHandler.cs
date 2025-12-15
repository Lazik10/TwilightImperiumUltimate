using System.Globalization;
using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class UpdateAsyncGameDataCommandHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<UpdateAsyncGameDataCommand, bool>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<bool> Handle(UpdateAsyncGameDataCommand request, CancellationToken cancellationToken)
    {
        var gameData = request.GameData;
        var gameIds = await _asyncStatsRepository.GetAsyncGameIds(cancellationToken);
        var finishedGameIds = await _asyncStatsRepository.GetAsyncFinishedGameIds(cancellationToken);

        foreach (var game in gameData)
        {
            // If the game is not in the database, add it
            if (!gameIds.Contains(game.AsyncGameID))
            {
                var gameStats = MapToGameStats(game);
                var result = await _asyncStatsRepository.AddGameStats(gameStats, cancellationToken);

                if (result == 0)
                    continue;

                foreach (var player in game.Players)
                {
                    var asyncPlayerProfile = await _asyncStatsRepository.GetOrCreateAsyncPlayerProfile(player.DiscordUserID, player.DiscordUserName, cancellationToken);

                    var playerStats = MapPlayerDataToPlayerStats(player, gameStats, game.Scoreboard, game.Winners);
                    await _asyncStatsRepository.AddPlayerStats(playerStats, cancellationToken);

                    await _asyncStatsRepository.AddAsyncPlayerProfileGameStats(asyncPlayerProfile.Id, gameStats.Id, cancellationToken);
                }
            }

            // If the game is already in the database and not finished, update it
            else if (!finishedGameIds.Contains(game.AsyncGameID))
            {
                var gameStats = await _asyncStatsRepository.GetAsyncGameByDiscordId(game.AsyncGameID, cancellationToken);

                if (gameStats == null)
                    continue;

                var success = await _asyncStatsRepository.UpdateGameStats(gameStats, game, cancellationToken);

                if (!success)
                {
                    // If the game stats could not be updated, remove the game from the database and parse it again in next cycle
                    await _asyncStatsRepository.DeleteGameStats(gameStats, cancellationToken);
                    continue;
                }

                foreach (var player in game.Players)
                {
                    var playerStats = MapPlayerDataToPlayerStats(player, gameStats, game.Scoreboard, game.Winners);
                    var playerStatsUpdateSuccess = await _asyncStatsRepository.UpdatePlayerStats(gameStats, playerStats, cancellationToken);

                    if (!playerStatsUpdateSuccess)
                    {
                        // If the player stats could not be updated, remove the game from the database and parse it again in next cycle
                        await _asyncStatsRepository.DeleteGameStats(gameStats, cancellationToken);
                        break;
                    }
                }
            }
        }

        return true;
    }

    private static GameStats MapToGameStats(GameData game)
    {
        var hasWinner = game.Players.Any(x => x.Score >= game.Scoreboard);
        var eventsFlag = TiglGalacticEventConverter.ConvertToFlags(game.Modes);

        var gameStats = new GameStats
        {
            AsyncGameID = game.AsyncGameID,
            AsyncFunGameName = game.AsyncFunGameName,
            Platform = game.Platform,
            Timestamp = game.SetupTimestamp,
            SetupTimestamp = game.SetupTimestamp,
            EndedTimestamp = game.EndedTimestamp,
            HasWinner = hasWinner,
            NumberOfPlayers = game.Players.Count,
            Round = game.Round,
            Turn = game.Turn,
            Scoreboard = game.Scoreboard,
            MapString = game.MapString,
            AbsolMode = game.AbsolMode,
            DiscordantStarsMode = game.DiscordantStarsMode,
            FrankenGame = game.FrankenGame,
            AllianceGame = game.AllianceMode,
            Homebrew = game.Homebrew,
            IsPoK = game.IsPoK,
            IsTigl = game.IsTigl,
            CreationEpochTimestamp = game.CreationEpochTimestamp,
            EndedEpochTimestamp = game.EndedEpochTimestamp,
            Completed = game.Completed,
            Events = eventsFlag,
            PlayerStatistics = [],
            GameStatistics = [],
        };

        return gameStats;
    }

    private static PlayerStats MapPlayerDataToPlayerStats(PlayerData player, GameStats gameStats, int gameVp, IReadOnlyCollection<string> winners)
    {
        var isWinner = winners.Contains(player.DiscordUserID);

        var playerStats = new PlayerStats
        {
            DiscordUserID = long.Parse(player.DiscordUserID, NumberStyles.Number, CultureInfo.InvariantCulture),
            DiscordUserName = player.DiscordUserName,
            FactionName = AsyncFactionParser.ParseFaction(player.FactionName),
            Score = player.Score > gameVp ? gameVp : player.Score,
            Color = player.ColorActual,
            TotalNumberOfTurns = player.TotalNumberOfTurns,
            TotalTurnTime = player.TotalTurnTime,
            ExpectedHits = player.ExpectedHits,
            ActualHits = player.ActualHits,
            Eliminated = player.Eliminated,
            Winner = isWinner,
            GameStatsId = gameStats.Id,
        };

        return playerStats;
    }
}

