using FluentResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class TiglRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<TiglRepository> logger)
    : ITiglRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<TiglRepository> _logger = logger;

    public async Task<List<TiglUser>> GetUsersFromGameReport(IGameReport gameReport, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(gameReport);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var users = await dbContext.TiglUsers
            .Include(x => x.AsyncStats)
            .ThenInclude(x => x.Rating)
            .Include(x => x.GlickoStats)
            .ThenInclude(x => x.Rating)
            .Include(x => x.TrueSkillStats)
            .ThenInclude(x => x.TrueSkillRating)
            .ToListAsync(cancellationToken);

        var gameUsers = users.Where(x => gameReport.PlayerResults
            .Any(y => y.DiscordId == x.DiscordId))
            .ToList();

        if (gameUsers.Count < gameReport.PlayerResults.Count)
        {
            _logger.LogWarning(
                "Not all users from the game report were found in the database for GameID: {GameId}",
                gameReport.GameId);
        }

        return gameUsers;
    }

    public async Task<MatchReport> InsertNewGameReport(MatchReport matchReport, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        try
        {
            matchReport.State = MatchState.AddedInQueue;
            dbContext.GameReports.Add(matchReport);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save new game report to the database for GameID: {GameId}", matchReport.GameId);
        }

        return matchReport;
    }

    public async Task<Result<int>> RegisterNewTiglUser(long discordId, string tiglUserName, CancellationToken cancellationToken, string discordTag = "", string ttsUserName = "", string ttpgUserName = "", string bgaUserName = "")
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
            TtsUserName = string.IsNullOrEmpty(ttsUserName) ? string.Empty : ttpgUserName,
            TtpgUserName = string.IsNullOrEmpty(ttpgUserName) ? string.Empty : ttpgUserName,
            BgaUserName = string.IsNullOrEmpty(bgaUserName) ? string.Empty : bgaUserName,
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

        var result = await CreateNewStatsForUser(newUser.Id);
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

    public async Task<int> GetCurrentSeason(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var currentSeason = await dbContext.Seasons
            .OrderByDescending(x => x.SeasonNumber)
            .Select(x => x.SeasonNumber)
            .FirstOrDefaultAsync(cancellationToken);

        return currentSeason;
    }

    public async Task<bool> CreateNewSeason(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var currentSeason = await GetCurrentSeason(cancellationToken);

        var newSeason = new Season
        {
            SeasonNumber = currentSeason + 1,
            Name = string.Empty,
        };

        try
        {
            dbContext.Seasons.Add(newSeason);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create a new season in the database.");
            return false;
        }
    }

    public async Task<Result<bool>> UpdateMatch(int matchId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var match = await dbContext.GameReports
            .FirstOrDefaultAsync(x => x.Id == matchId, cancellationToken);

        if (match is null)
        {
            _logger.LogWarning("Match with ID: {MatchId} not found.", matchId);
            return Result.Fail<bool>("Match not found.");
        }

        try
        {
            match.State = MatchState.Evaluated;
            dbContext.GameReports.Update(match);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update match with ID: {MatchId}", matchId);
            return Result.Fail<bool>("Failed to update match.");
        }
    }

    public async Task<TiglParameter?> GetTiglParameter(TiglParameterName parameterName, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var parameter = await dbContext.TiglParameters
            .FirstOrDefaultAsync(x => x.Name == parameterName, cancellationToken);

        return parameter;
    }

    public async Task<MatchReport?> GetMatchReport(string gameId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var matchReport = await dbContext.GameReports.FirstOrDefaultAsync(x => x.GameId == gameId, cancellationToken);
        return matchReport;
    }

    private async Task<bool> CreateNewStatsForUser(int id)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var asyncStats = new List<AsyncStats>()
        {
            new AsyncStats() { TiglUserId = id, League = TiglLeague.Tigl, Rating = new AsyncRating() },
            new AsyncStats() { TiglUserId = id, League = TiglLeague.Homebrew, Rating = new AsyncRating() },
            new AsyncStats() { TiglUserId = id, League = TiglLeague.Open, Rating = new AsyncRating() },
        };

        var glickoStats = new List<GlickoStats>()
        {
            new GlickoStats() { TiglUserId = id, League = TiglLeague.Tigl, Rating = new GlickoRating() },
            new GlickoStats() { TiglUserId = id, League = TiglLeague.Homebrew, Rating = new GlickoRating() },
            new GlickoStats() { TiglUserId = id, League = TiglLeague.Open, Rating = new GlickoRating() },
        };

        var trueSkillStats = new List<TrueSkillStats>()
        {
            new TrueSkillStats() { TiglUserId = id, League = TiglLeague.Tigl, TrueSkillRating = new TrueSkillRating() },
            new TrueSkillStats() { TiglUserId = id, League = TiglLeague.Homebrew, TrueSkillRating = new TrueSkillRating() },
            new TrueSkillStats() { TiglUserId = id, League = TiglLeague.Open, TrueSkillRating = new TrueSkillRating() },
        };

        dbContext.AsyncStats.AddRange(asyncStats);
        dbContext.GlickoStats.AddRange(glickoStats);
        dbContext.TrueSkillStats.AddRange(trueSkillStats);

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
