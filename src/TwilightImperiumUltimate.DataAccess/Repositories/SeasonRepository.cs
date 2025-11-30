using FluentResults;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class SeasonRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<SeasonRepository> logger)
    : ISeasonRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<SeasonRepository> _logger = logger;

    public async Task<int> GetCurrentSeason(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var currentSeason = await dbContext.Seasons.FirstAsync(x => x.IsActive, cancellationToken);

        return currentSeason.SeasonNumber;
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

    public async Task<Result<Season>> EndCurrentSeason(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var currentSeason = await dbContext.Seasons.FirstOrDefaultAsync(x => x.IsActive, cancellationToken);
        if (currentSeason is null)
            return Result.Fail<Season>("Can't end season, because there is no active season!");

        currentSeason.IsActive = false;
        currentSeason.EndDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var nextSeason = await dbContext.Seasons.FirstOrDefaultAsync(x => x.SeasonNumber == currentSeason.SeasonNumber + 1, cancellationToken: cancellationToken);
        if (nextSeason is not null)
        {
            nextSeason.IsActive = true;
            nextSeason.StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
            dbContext.Seasons.Update(nextSeason);
            dbContext.Seasons.Update(currentSeason);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to end current season {SeasonNumber}", currentSeason.SeasonNumber);
                return Result.Fail<Season>($"Failed to end current season {currentSeason.SeasonNumber}. Exception: {ex}");
            }

            return Result.Ok(currentSeason);
        }

        var newSeason = new Season
        {
            SeasonNumber = currentSeason.SeasonNumber + 1,
            Name = string.Empty,
            IsActive = true,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
        };

        dbContext.Seasons.Update(currentSeason);
        dbContext.Seasons.Add(newSeason);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to end current season {SeasonNumber}", currentSeason.SeasonNumber);
            return Result.Fail<Season>($"Failed to end current season {currentSeason.SeasonNumber}. Exception: {ex}");
        }

        return Result.Ok(currentSeason);
    }

    public async Task<Result<Season>> SetActiveSeason(int seasonNumber, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var newActiveSeason = await dbContext.Seasons
            .FirstOrDefaultAsync(s => s.SeasonNumber == seasonNumber, cancellationToken);

        if (newActiveSeason is null)
            return Result.Fail<Season>($"Season {seasonNumber} does not exist.");

        var currentSeason = await dbContext.Seasons
            .FirstOrDefaultAsync(s => s.IsActive, cancellationToken);

        if (currentSeason is not null)
        {
            if (currentSeason.SeasonNumber == seasonNumber)
                return Result.Ok(currentSeason);

            currentSeason.IsActive = false;
            dbContext.Seasons.Update(currentSeason);
        }

        newActiveSeason.IsActive = true;
        dbContext.Seasons.Update(newActiveSeason);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set new active season {SeasonNumber}", newActiveSeason.SeasonNumber);
            return Result.Fail<Season>($"Failed to set new active season {newActiveSeason.SeasonNumber}. Exception: {ex}");
        }

        return Result.Ok(newActiveSeason);
    }

    public async Task<Result<Season>> AddNewSeason(int seasonNumber, string seasonName, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var newSeason = new Season
        {
            SeasonNumber = seasonNumber,
            Name = seasonName,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsActive = false,
        };

        dbContext.Seasons.Add(newSeason);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add new season {SeasonNumber}", newSeason.SeasonNumber);
            return Result.Fail<Season>($"Failed to add new season {newSeason.SeasonNumber}. Exception: {ex}");
        }

        return Result.Ok(newSeason);
    }

    public async Task<IReadOnlyCollection<Season>> GetAllSeasons(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var seasons = await dbContext.Seasons
            .OrderBy(s => s.SeasonNumber)
            .ToListAsync(cancellationToken);

        return seasons;
    }

    public async Task<Season?> GetSeasonByNumber(int seasonNumber, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Seasons.FirstOrDefaultAsync(s => s.SeasonNumber == seasonNumber, cancellationToken);
    }

    public async Task<bool> UpdateSeason(Season season, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        dbContext.Seasons.Update(season);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update season {SeasonNumber}", season.SeasonNumber);
            return false;
        }
    }

    public async Task<MatchReport?> GetFastestGameInSeason(int season, TiglLeague league, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var fastestGame = await dbContext.GameReports
            .Where(gr => gr.Season == season && gr.League == league)
            .OrderBy(gr => gr.EndTimestamp - gr.StartTimestamp)
            .Include(gr => gr.PlayerResults)
            .FirstOrDefaultAsync(cancellationToken);

        return fastestGame;
    }

    public async Task<MatchReport?> GetSlowestGameInSeason(int season, TiglLeague league, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var slowestGame = await dbContext.GameReports
            .Where(gr => gr.Season == season && gr.League == league)
            .OrderByDescending(gr => gr.EndTimestamp - gr.StartTimestamp)
            .Include(gr => gr.PlayerResults)
            .FirstOrDefaultAsync(cancellationToken);

        return slowestGame;
    }
}
