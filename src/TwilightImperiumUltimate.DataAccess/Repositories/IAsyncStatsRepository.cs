using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IAsyncStatsRepository
{
    Task<HashSet<string>> GetAsyncGameIds(CancellationToken cancellationToken);

    Task<HashSet<string>> GetAsyncFinishedGameIds(CancellationToken cancellationToken);

    Task<List<GameStats>> GetAllAsyncGames(CancellationToken cancellationToken);

    Task<List<GameStats>> GetAllAsyncGamesByYearAndMonthQuery(int year, int month, CancellationToken cancellationToken);

    Task<List<long>> GetAllAsyncGameDates(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<string>> GetAllAsyncGameNames(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<string>> GetAllAsyncFunGameNames(CancellationToken cancellationToken);

    Task<GameStats?> GetAsyncGameByDiscordId(string asyncGameId, CancellationToken cancellationToken);

    Task<GameStats?> GetAsyncGameByFunName(string funName, CancellationToken cancellationToken);

    Task<bool> UpdateGameStats(GameStats gameStats, GameData gameData, CancellationToken cancellationToken);

    Task<bool> DeleteGameStats(GameStats gameStats, CancellationToken cancellationToken);

    Task<int> AddGameStats(GameStats gameStats, CancellationToken cancellationToken);

    Task<int> AddPlayerStats(PlayerStats playerStats, CancellationToken cancellationToken);

    Task<bool> UpdatePlayerStats(GameStats gameStats, PlayerStats newPlayerStats, CancellationToken cancellationToken);

    Task<int> AddAsyncPlayerProfileGameStats(int asyncPlayerProfileId, int gameStatsId, CancellationToken cancellationToken);

    Task<AsyncPlayerProfile> GetOrCreateAsyncPlayerProfile(string playerDiscordId, string playerUserName, CancellationToken cancellationToken);

    Task<AsyncPlayerProfile?> GetAsyncPlayerProfileByDiscordId(long discordUserID, CancellationToken cancellationToken);

    Task<AsyncPlayerProfile?> GetAsyncPlayerProfileByPlayerRequest(long discordUserID, string playerUserName, long playerId, CancellationToken cancellationToken);

    Task<int> UpdateAsyncPlayerProfile(AsyncPlayerProfile asyncPlayerProfile, CancellationToken cancellationToken);

    Task<List<AsyncPlayerProfile>> GetAllAsyncPlayerProfiles(bool withExcludedProfiles, CancellationToken cancellationToken);

    Task<bool> UpdateAsyncPlayerProfileSettings(long asyncPlayerDiscordId, AsyncPlayerProfileSettings settings, CancellationToken cancellationToken);

    Task<AsyncPlayerProfile?> GetPlayerIdByDiscordId(long discordId, CancellationToken cancellationToken);
}
