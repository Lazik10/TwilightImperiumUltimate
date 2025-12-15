using Microsoft.Extensions.Logging;
using System.Globalization;
using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class AsyncStatsRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<AsyncStatsRepository> logger)
    : IAsyncStatsRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<AsyncStatsRepository> _logger = logger;

    public async Task<HashSet<string>> GetAsyncGameIds(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return dbContext.GameStats
            .Select(x => x.AsyncGameID)
            .ToHashSet();
    }

    public async Task<HashSet<string>> GetAsyncFinishedGameIds(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return dbContext.GameStats
            .Where(x => x.EndedTimestamp.HasValue)
            .Select(x => x.AsyncGameID)
            .ToHashSet();
    }

    public async Task<List<GameStats>> GetAllAsyncGames(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.GameStats
            .Include(x => x.PlayerStatistics)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<GameStats>> GetAllAsyncGamesByYearAndMonthQuery(int year, int month, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var startOfMonth = new DateTimeOffset(new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc)).ToUnixTimeSeconds();

        // Adjust end search to next year's January
        if (month == 12)
        {
            year++;
            month = 1;
        }

        var startOfNextMonth = new DateTimeOffset(new DateTime(year, month + 1, 1, 0, 0, 0, DateTimeKind.Utc)).ToUnixTimeSeconds();

        return await dbContext.GameStats
            .Where(x =>
                x.SetupTimestamp >= startOfMonth && x.SetupTimestamp < startOfNextMonth)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<long>> GetAllAsyncGameDates(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.GameStats
            .Select(x => x.SetupTimestamp)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<string>> GetAllAsyncGameNames(CancellationToken cancellationToken)
    {
        await using var dbContex = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContex.GameStats.Select(x => x.AsyncGameID).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<string>> GetAllAsyncFunGameNames(CancellationToken cancellationToken)
    {
        await using var dbContex = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContex.GameStats.Select(x => x.AsyncFunGameName).ToListAsync(cancellationToken);
    }

    public async Task<GameStats?> GetAsyncGameByDiscordId(string asyncGameId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.GameStats
            .FirstOrDefaultAsync(x => x.AsyncGameID == asyncGameId, cancellationToken);
    }

    public async Task<GameStats?> GetAsyncGameByFunName(string funName, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.GameStats
            .FirstOrDefaultAsync(x => x.AsyncFunGameName == funName && !string.IsNullOrEmpty(x.AsyncFunGameName), cancellationToken);
    }

    public async Task<bool> UpdateGameStats(GameStats gameStats, GameData gameData, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var game = await dbContext.GameStats
            .FirstOrDefaultAsync(x => x.AsyncGameID == gameStats.AsyncGameID, cancellationToken);

        if (game is not null)
        {
            var updatedGameStats = UpdateGameStats(gameStats, gameData);
            dbContext.GameStats.Update(updatedGameStats);
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update GameStats for game with AsyncGameID: {AsyncGameID}", gameStats.AsyncGameID);
                return false;
            }
        }

        return false;
    }

    public async Task<bool> DeleteGameStats(GameStats gameStats, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var game = await dbContext.GameStats
            .FirstOrDefaultAsync(x => x.AsyncGameID == gameStats.AsyncGameID, cancellationToken);

        if (game is not null)
        {
            dbContext.GameStats.Remove(game);
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Deleted GameStats for game with AsyncGameID: {AsyncGameID}", gameStats.AsyncGameID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete GameStats for game with AsyncGameID: {AsyncGameID}", gameStats.AsyncGameID);
                return false;
            }
        }

        return false;
    }

    public async Task<int> AddGameStats(GameStats gameStats, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var game = await dbContext.GameStats
            .FirstOrDefaultAsync(x => x.AsyncGameID == gameStats.AsyncGameID, cancellationToken);

        if (game is null)
        {
            dbContext.GameStats.Add(gameStats);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        return 0;
    }

    public async Task<int> AddPlayerStats(PlayerStats playerStats, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        dbContext.PlayerStats.Add(playerStats);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdatePlayerStats(GameStats gameStats, PlayerStats newPlayerStats, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var oldPlayerStats = await dbContext.PlayerStats
            .FirstOrDefaultAsync(x => x.FactionName == newPlayerStats.FactionName && x.Color == newPlayerStats.Color && x.GameStatsId == gameStats.Id, cancellationToken);

        if (oldPlayerStats is not null)
        {
            var newPlayerProfile = await dbContext.AsyncPlayerProfiles
                .FirstOrDefaultAsync(x => x.DiscordUserId == newPlayerStats.DiscordUserID, cancellationToken);
            var oldPlayerProfile = await dbContext.AsyncPlayerProfiles
                .FirstOrDefaultAsync(x => x.DiscordUserId == oldPlayerStats.DiscordUserID, cancellationToken);

            if (newPlayerProfile is not null && oldPlayerProfile is not null && oldPlayerStats.DiscordUserID != newPlayerStats.DiscordUserID)
            {
                await RemoveAsyncPlayerProfileGameStats(oldPlayerProfile.Id, gameStats.Id, cancellationToken);
                await AddAsyncPlayerProfileGameStats(newPlayerProfile.Id, gameStats.Id, cancellationToken);
            }
        }

        if (oldPlayerStats is not null)
        {
            var updatedPlayerStats = UpdatePlayerStats(newPlayerStats, oldPlayerStats);

            dbContext.PlayerStats.Update(updatedPlayerStats);
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update PlayerStats for game with AsyncGameID: {AsyncGameID} for Player: {Plyer} and Faction: {Faction}",
                    gameStats.AsyncGameID,
                    newPlayerStats.DiscordUserName,
                    newPlayerStats.FactionName.ToString());
                return false;
            }
        }
        else
        {
            _logger.LogError(
                "Failed to update PlayerStats for game with AsyncGameID: {AsyncGameID} for Player: {Plyer} and Faction: {Faction} because stats are missing!",
                gameStats.AsyncGameID,
                newPlayerStats.DiscordUserName,
                newPlayerStats.FactionName.ToString());
        }

        return false;
    }

    public async Task<int> AddAsyncPlayerProfileGameStats(int asyncPlayerProfileId, int gameStatsId, CancellationToken cancellationToken)
    {
        var asyncPlayerProfileGameStats = new AsyncPlayerProfileGameStats
        {
            AsyncPlayerProfileId = asyncPlayerProfileId,
            GameStatsId = gameStatsId,
        };

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var existingAsyncPlayerProfileGameStats = await dbContext.AsyncPlayerProfileGameStats
            .FirstOrDefaultAsync(x => x.AsyncPlayerProfileId == asyncPlayerProfileId && x.GameStatsId == gameStatsId, cancellationToken);

        if (existingAsyncPlayerProfileGameStats is null)
        {
            try
            {
                dbContext.AsyncPlayerProfileGameStats.Add(asyncPlayerProfileGameStats);
                return await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add AsyncPlayerProfileGameStats for game with GameStatsId: {GameStatsId} and AsyncPlayerProfileId: {AsyncPlayerProfileId}",
                    gameStatsId,
                    asyncPlayerProfileId);
                return 0;
            }
        }
        else
        {
            _logger.LogError("Cannot add AsyncPlayerProfileGameStats for game with GameStatsId: {GameStatsId} and AsyncPlayerProfileId: {AsyncPlayerProfileId} because it already exists", gameStatsId, asyncPlayerProfileId);
        }

        return 0;
    }

    public async Task<int> RemoveAsyncPlayerProfileGameStats(int asyncPlayerProfileId, int gameStatsId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var asyncPlayerProfileGameStats = await dbContext.AsyncPlayerProfileGameStats
            .FirstOrDefaultAsync(x => x.AsyncPlayerProfileId == asyncPlayerProfileId && x.GameStatsId == gameStatsId, cancellationToken);

        if (asyncPlayerProfileGameStats is null)
            return 0;

        try
        {
            dbContext.AsyncPlayerProfileGameStats.Remove(asyncPlayerProfileGameStats);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to remove AsyncPlayerProfileGameStats for game with GameStatsId: {GameStatsId} and AsyncPlayerProfileId: {AsyncPlayerProfileId}",
                gameStatsId,
                asyncPlayerProfileId);
            return 0;
        }
    }

    public async Task<AsyncPlayerProfile?> GetAsyncPlayerProfileByDiscordId(long discordUserID, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.AsyncPlayerProfiles
            .AsSplitQuery()
            .Include(x => x.ProfileSettings)
            .Include(x => x.GameStatistics)
            .ThenInclude(x => x.GameStats)
            .ThenInclude(x => x.PlayerStatistics)
            .FirstOrDefaultAsync(x => x.DiscordUserId == discordUserID, cancellationToken);
    }

    public async Task<AsyncPlayerProfile?> GetAsyncPlayerProfileByPlayerRequest(long discordUserID, string playerUserName, long playerId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var player = await dbContext.AsyncPlayerProfiles
            .AsSplitQuery()
            .Include(x => x.ProfileSettings)
            .Include(x => x.GameStatistics)
            .ThenInclude(x => x.GameStats)
            .ThenInclude(x => x.PlayerStatistics)
            .FirstOrDefaultAsync(x => x.DiscordUserId == discordUserID || x.DiscordUserName == playerUserName, cancellationToken);

        player ??= await dbContext.AsyncPlayerProfiles
            .AsSplitQuery()
            .Include(x => x.ProfileSettings)
            .Include(x => x.GameStatistics)
            .ThenInclude(x => x.GameStats)
            .ThenInclude(x => x.PlayerStatistics)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        return player;
    }

    public async Task<AsyncPlayerProfile?> GetPlayerIdByDiscordId(long discordId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.AsyncPlayerProfiles
            .FirstOrDefaultAsync(x => x.DiscordUserId == discordId, cancellationToken);
    }

    public async Task<int> UpdateAsyncPlayerProfile(AsyncPlayerProfile asyncPlayerProfile, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        dbContext.AsyncPlayerProfiles.Update(asyncPlayerProfile);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<AsyncPlayerProfile> GetOrCreateAsyncPlayerProfile(string playerDiscordId, string playerUserName, CancellationToken cancellationToken)
    {
        var longPlayerDiscordId = long.Parse(playerDiscordId, CultureInfo.InvariantCulture);
        var asyncPlayerProfile = await GetAsyncPlayerProfileByDiscordId(longPlayerDiscordId, cancellationToken);
        if (asyncPlayerProfile is null)
        {
            asyncPlayerProfile = new AsyncPlayerProfile
            {
                DiscordUserId = longPlayerDiscordId,
                DiscordUserName = playerUserName,
                GameStatistics = [],
                ProfileSettings = new AsyncPlayerProfileSettings(),
            };

            var success = await CreateNewAsyncPlayerProfile(asyncPlayerProfile, cancellationToken);
            if (success == 0)
            {
                _logger.LogError("Failed to create a new AsyncPlayerProfile for player with DiscordUserID: {DiscordUserID}", playerDiscordId);
            }
            else
            {
                _logger.LogInformation("Created a new AsyncPlayerProfile for player with DiscordUserID: {DiscordUserID}", playerDiscordId);
            }
        }
        else if (asyncPlayerProfile.DiscordUserName != playerUserName)
        {
            asyncPlayerProfile.DiscordUserName = playerUserName;
            await UpdateAsyncPlayerProfile(asyncPlayerProfile, cancellationToken);
        }

        return asyncPlayerProfile;
    }

    public async Task<List<AsyncPlayerProfile>> GetAllAsyncPlayerProfiles(bool withExcludedProfiles, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var userProfiles = await dbContext.AsyncPlayerProfiles
            .Include(x => x.ProfileSettings)
            .ToListAsync(cancellationToken);

        return withExcludedProfiles ?
            userProfiles :
            userProfiles.Where(x => !x.ProfileSettings!.ExcludeFromAsyncStats).ToList();
    }

    public async Task<bool> UpdateAsyncPlayerProfileSettings(long asyncPlayerDiscordId, AsyncPlayerProfileSettings settings, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var playerProfile = await dbContext.AsyncPlayerProfiles
            .Include(x => x.ProfileSettings)
            .FirstOrDefaultAsync(x => x.DiscordUserId == asyncPlayerDiscordId, cancellationToken);

        if (playerProfile is null)
            return false;

        var playerSettings = playerProfile.ProfileSettings;
        if (playerSettings is null)
            return false;

        playerSettings.ExcludeFromAsyncStats = settings.ExcludeFromAsyncStats;
        playerSettings.ShowWinRate = settings.ShowWinRate;
        playerSettings.ShowTurnStats = settings.ShowTurnStats;
        playerSettings.ShowCombatStats = settings.ShowCombatStats;
        playerSettings.ShowVpStats = settings.ShowVpStats;
        playerSettings.ShowFactionStats = settings.ShowFactionStats;
        playerSettings.ShowOpponents = settings.ShowOpponents;
        playerSettings.ShowGames = settings.ShowGames;

        try
        {
            dbContext.AsyncPlayerStatisticsSettings.Update(playerSettings);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update AsyncPlayerProfileSettings for player with DiscordUserID: {DiscordUserID}", asyncPlayerDiscordId);
            return false;
        }
    }

    private async Task<int> CreateNewAsyncPlayerProfile(AsyncPlayerProfile asyncPlayerProfile, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        dbContext.AsyncPlayerProfiles.Add(asyncPlayerProfile);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    private GameStats UpdateGameStats(GameStats gameStats, GameData gameData)
    {
        gameStats.Platform = gameData.Platform;
        gameStats.Timestamp = gameData.SetupTimestamp;
        gameStats.SetupTimestamp = gameData.SetupTimestamp;
        gameStats.EndedTimestamp = gameData.EndedTimestamp;
        gameStats.HasWinner = gameData.Players.Any(x => x.Score >= gameData.Scoreboard);
        gameStats.NumberOfPlayers = gameData.Players.Count;
        gameStats.Round = gameData.Round;
        gameStats.Turn = gameData.Turn;
        gameStats.Scoreboard = gameData.Scoreboard;
        gameStats.MapString = gameData.MapString;
        gameStats.AbsolMode = gameData.AbsolMode;
        gameStats.DiscordantStarsMode = gameData.DiscordantStarsMode;
        gameStats.FrankenGame = gameData.FrankenGame;
        gameStats.Homebrew = gameData.Homebrew;
        gameStats.IsPoK = gameData.IsPoK;
        gameStats.IsTigl = gameData.IsTigl;

        return gameStats;
    }

    private PlayerStats UpdatePlayerStats(PlayerStats newPlayerStats, PlayerStats oldPlayerStats)
    {
        oldPlayerStats.DiscordUserID = newPlayerStats.DiscordUserID;
        oldPlayerStats.DiscordUserName = newPlayerStats.DiscordUserName;
        oldPlayerStats.Score = newPlayerStats.Score;
        oldPlayerStats.Color = newPlayerStats.Color;
        oldPlayerStats.TotalNumberOfTurns = newPlayerStats.TotalNumberOfTurns;
        oldPlayerStats.TotalTurnTime = newPlayerStats.TotalTurnTime;
        oldPlayerStats.ExpectedHits = newPlayerStats.ExpectedHits;
        oldPlayerStats.ActualHits = newPlayerStats.ActualHits;
        oldPlayerStats.Eliminated = newPlayerStats.Eliminated;
        oldPlayerStats.Winner = newPlayerStats.Winner;

        return oldPlayerStats;
    }
}
