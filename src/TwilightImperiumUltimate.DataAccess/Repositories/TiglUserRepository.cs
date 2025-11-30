using FluentResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class TiglUserRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<TiglUserRepository> logger)
    : ITiglUserRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<TiglUserRepository> _logger = logger;

    public async Task<List<TiglUser>> GetUsersFromGameReport(IGameReport gameReport, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(gameReport);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var userIds = gameReport.PlayerResults.Select(x => x.DiscordId).ToList();

        var users = await dbContext.TiglUsers
            .Where(x => userIds.Contains(x.DiscordId))
            .Include(x => x.AsyncStats!.Where(x => x.League == gameReport.League))
                .ThenInclude(x => x.Rating)
            .Include(x => x.GlickoStats!.Where(x => x.League == gameReport.League))
                .ThenInclude(x => x.Rating)
            .Include(x => x.TrueSkillStats!.Where(x => x.League == gameReport.League))
                .ThenInclude(x => x.TrueSkillRating)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        if (users.Count < gameReport.PlayerResults.Count)
        {
            _logger.LogWarning(
                "Not all users from the game report were found in the database for GameID: {GameId}",
                gameReport.GameId);
        }

        return users;
    }

    public async Task<List<MatchReport>> GetTiglUserMatchReports(
    int tiglUserId,
    long endTimestamp,
    CancellationToken cancellationToken,
    bool onlyWins = false,
    HashSet<TiglFactionName>? factions = null,
    int count = int.MaxValue)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var query = dbContext.GameReports
            .Where(gr => gr.EndTimestamp < endTimestamp &&
                (gr.League == TiglLeague.ThundersEdge || gr.League == TiglLeague.Fractured)
                && gr.PlayerResults.Any(pr => pr.TiglUserId == tiglUserId))
            .OrderByDescending(x => x.EndTimestamp)
            .Take(count)
            .Include(gr => gr.PlayerResults)
            .AsNoTracking();

        if (onlyWins)
        {
            query = query.Where(gr => gr.PlayerResults.Any(x => x.IsWinner && x.TiglUserId == tiglUserId));
        }

        if (factions is not null && factions.Count > 0)
        {
            query = query.Where(gr => gr.PlayerResults.Any(x => factions.Contains(x.Faction) && x.TiglUserId == tiglUserId));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Result<int>> RegisterNewTiglUser(long discordId, string tiglUserName, long startGameTimestamp, CancellationToken cancellationToken, string discordTag = "", string ttsUserName = "", string ttpgUserName = "", string bgaUserName = "")
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var users = await dbContext.TiglUsers.ToListAsync(cancellationToken);

        if (users.FirstOrDefault(x => x.DiscordId == discordId) is not null)
            return Result.Fail<int>("User with this DiscordID already exists!");

        if (users.FirstOrDefault(x => x.DiscordTag == discordTag) is not null)
            return Result.Fail<int>("User with this DiscordTag already exists!");

        if (users.FirstOrDefault(x => x.TiglUserName == tiglUserName) is not null)
            return Result.Fail<int>("User with this TIGL Username already exists!");

        var newUser = new TiglUser
        {
            DiscordId = discordId,
            TiglUserName = tiglUserName,
            DiscordTag = string.IsNullOrEmpty(discordTag) ? string.Empty : discordTag,
            TtsUserName = string.IsNullOrEmpty(ttsUserName) ? string.Empty : ttsUserName,
            TtpgUserName = string.IsNullOrEmpty(ttpgUserName) ? string.Empty : ttpgUserName,
            BgaUserName = string.IsNullOrEmpty(bgaUserName) ? string.Empty : bgaUserName,
            ProphecyOfKingsRank = TiglRankName.Unranked,
        };

        try
        {
            dbContext.TiglUsers.Add(newUser);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                _logger.LogError(sqlEx, "Unique constraint violation while saving Tigl user: {DiscordId}, {TiglUserName}", discordId, tiglUserName);
                return Result.Fail<int>("Unique constraint violation. The user already exists.");
            }

            _logger.LogError(dbEx, "Database update exception while saving Tigl user: {DiscordId}, {TiglUserName}", discordId, tiglUserName);
            return Result.Fail<int>("Database update exception occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to save new Tigl user to the database for DiscordId: {DiscordId}, TiglUserName: {TiglUserName}, DiscordTag: {DiscordTag}, TtsUserName: {TtsUserName}, Ttpg: {TtpgUserName}, BgaUserName: {BgaUserName}",
                discordId,
                tiglUserName,
                discordTag,
                ttsUserName,
                ttpgUserName,
                bgaUserName);
            return Result.Fail<int>("Failed to register new Tigl user.");
        }

        var result = await CreateNewStatsForUser(newUser.Id, startGameTimestamp);
        if (!result)
        {
            _logger.LogError("Failed to create new stats for Tigl user with ID: {Id}", newUser.Id);
            return Result.Fail<int>("Failed to create new stats for the user.");
        }

        return Result.Ok(newUser.Id);
    }

    public async Task<Result<bool>> ChangeTiglUserName(long discordId, string newTiglUserName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(newTiglUserName);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var user = await dbContext.TiglUsers
            .FirstOrDefaultAsync(x => x.DiscordId == discordId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Tigl user with DiscordId: {DiscordId} not found.", discordId);
            return Result.Fail<bool>("User not found.");
        }

        try
        {
            user.TiglUserName = newTiglUserName;
            user.TiglUserNameChanged = true;
            user.LastUserNameChange = DateOnly.FromDateTime(DateTime.UtcNow);

            dbContext.TiglUsers.Update(user);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to change Tigl username for DiscordId: {DiscordId}", discordId);
            return Result.Fail<bool>("Failed to change Tigl username. Username already exists.");
        }
    }

    public async Task<TiglUser?> FindTiglUserFromGameReportPlayerResult(IPlayerResult playerResult, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var users = await dbContext.TiglUsers.ToListAsync(cancellationToken);

        var gameUser = users.Find(x => playerResult.DiscordId == x.DiscordId);

        return gameUser;
    }

    public async Task<TiglUser?> GetTiglUserById(int id, TiglLeague league, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var tiglUser = await dbContext.TiglUsers
            .Include(x => x.TiglRanks)
            .Include(x => x.AsyncStats!.Where(x => x.League == league))
                .ThenInclude(x => x.MatchStats)
            .Include(x => x.GlickoStats!.Where(x => x.League == league))
                .ThenInclude(x => x.MatchStats)
            .Include(x => x.TrueSkillStats!.Where(x => x.League == league))
                .ThenInclude(x => x.MatchStats)
            .AsSplitQuery()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        return tiglUser;
    }

    public async Task<IReadOnlyCollection<TiglUser?>> GetTiglUsersBaseInfoById(IReadOnlyCollection<int> tiglUserIds, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var tiglUsers = await dbContext.TiglUsers
            .Where(x => tiglUserIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return tiglUsers;
    }

    public async Task<TiglUser?> GetTiglUserByDiscordId(long discordId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var user = await dbContext.TiglUsers.FirstOrDefaultAsync(x => x.DiscordId == discordId, cancellationToken);
        return user;
    }

    public async Task UpdateTiglPlayers(IReadOnlyCollection<TiglUser> players, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        try
        {
            dbContext.TiglUsers.UpdateRange(players);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Tigl players in the database.");
        }
    }

    public async Task InsertPlayerMatchStats(IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        try
        {
            dbContext.AsyncPlayerMatchStats.AddRange(matchStats);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to insert player match stats for game: {GameId} into the database.", matchStats.FirstOrDefault()?.Match?.GameId);
        }
    }

    public async Task InsertPlayerMatchStats(IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        try
        {
            dbContext.GlickoPlayerMatchStats.AddRange(matchStats);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to insert player match stats for game: {GameId} into the database.", matchStats.FirstOrDefault()?.Match?.GameId);
        }
    }

    public async Task InsertPlayerMatchStats(IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        try
        {
            dbContext.TrueSkillPlayerMatchStats.AddRange(matchStats);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to insert player match stats for game: {GameId} into the database.", matchStats.FirstOrDefault()?.Match?.GameId);
        }
    }

    public async Task<HashSet<int>> GetAllTiglUserIds(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var userIds = dbContext.TiglUsers
            .Select(u => u.Id)
            .ToHashSet();

        return userIds;
    }

    public async Task<HashSet<int>> GetActiveUserIds(int season, TiglLeague league, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var activeUserIds = dbContext.GameReports
            .Include(x => x.PlayerResults)
            .Where(u => u.Season == season && u.League == league)
            .SelectMany(u => u.PlayerResults.Select(pr => pr.TiglUserId))
            .ToHashSet();

        return activeUserIds;
    }

    public async Task<List<TiglUser>> GetUsersByIds(IReadOnlyCollection<int> tiglUserIds, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var users = await dbContext.TiglUsers
            .Where(u => tiglUserIds.Contains(u.Id))
            .Include(u => u.AsyncStats!)
                .ThenInclude(s => s.Rating)
            .Include(u => u.GlickoStats!)
                .ThenInclude(s => s.Rating)
            .Include(u => u.TrueSkillStats!)
                .ThenInclude(s => s.TrueSkillRating)
            .Include(x => x.TiglRanks)
            .ToListAsync(cancellationToken);

        return users;
    }

    private async Task<bool> CreateNewStatsForUser(int id, long startGameTimestamp)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var asyncStats = new List<AsyncStats>()
        {
            new AsyncStats() { TiglUserId = id, League = TiglLeague.Test, Rating = new AsyncRating() },
            new AsyncStats() { TiglUserId = id, League = TiglLeague.ProphecyOfKings, Rating = new AsyncRating() },
            new AsyncStats() { TiglUserId = id, League = TiglLeague.ThundersEdge, Rating = new AsyncRating() },
            new AsyncStats() { TiglUserId = id, League = TiglLeague.Fractured, Rating = new AsyncRating() },
        };

        var glickoStats = new List<GlickoStats>()
        {
            new GlickoStats() { TiglUserId = id, League = TiglLeague.Test, Rating = new GlickoRating() },
            new GlickoStats() { TiglUserId = id, League = TiglLeague.ProphecyOfKings, Rating = new GlickoRating() },
            new GlickoStats() { TiglUserId = id, League = TiglLeague.ThundersEdge, Rating = new GlickoRating() },
            new GlickoStats() { TiglUserId = id, League = TiglLeague.Fractured, Rating = new GlickoRating() },
        };

        var trueSkillStats = new List<TrueSkillStats>()
        {
            new TrueSkillStats() { TiglUserId = id, League = TiglLeague.Test, TrueSkillRating = new TrueSkillRating() },
            new TrueSkillStats() { TiglUserId = id, League = TiglLeague.ProphecyOfKings, TrueSkillRating = new TrueSkillRating() },
            new TrueSkillStats() { TiglUserId = id, League = TiglLeague.ThundersEdge, TrueSkillRating = new TrueSkillRating() },
            new TrueSkillStats() { TiglUserId = id, League = TiglLeague.Fractured, TrueSkillRating = new TrueSkillRating() },
        };

        var prophecyRank = new TiglRank() { TiglUserId = id, Name = TiglRankName.Unranked, League = TiglLeague.ProphecyOfKings, AchievedAt = startGameTimestamp - 1 };
        var thundersEdgeRank = new TiglRank() { TiglUserId = id, Name = TiglRankName.Unranked, League = TiglLeague.ThundersEdge, AchievedAt = startGameTimestamp - 1 };
        var shatteredRank = new TiglRank() { TiglUserId = id, Name = TiglRankName.Unranked, League = TiglLeague.Fractured, AchievedAt = startGameTimestamp - 1 };

        dbContext.AsyncStats.AddRange(asyncStats);
        dbContext.GlickoStats.AddRange(glickoStats);
        dbContext.TrueSkillStats.AddRange(trueSkillStats);
        dbContext.Ranks.AddRange(prophecyRank, thundersEdgeRank, shatteredRank);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create new stats for Tigl user with ID: {Id}", id);
            return false;
        }

        foreach (var asyncStat in asyncStats.Where(asyncStat => asyncStat.Rating is not null))
        {
            asyncStat.AsyncRatingId = asyncStat.Rating!.Id;
        }

        foreach (var glickoStat in glickoStats.Where(glickoStat => glickoStat.Rating is not null))
        {
            glickoStat.GlickoRatingId = glickoStat.Rating!.Id;
        }

        foreach (var trueSkillStat in trueSkillStats.Where(trueSkillStat => trueSkillStat.TrueSkillRating is not null))
        {
            trueSkillStat.TrueSkillRatingId = trueSkillStat.TrueSkillRating!.Id;
        }

        dbContext.AsyncStats.UpdateRange(asyncStats);
        dbContext.GlickoStats.UpdateRange(glickoStats);
        dbContext.TrueSkillStats.UpdateRange(trueSkillStats);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update new stats for Tigl user with ID: {Id}", id);
            return false;
        }

        return true;
    }
}
