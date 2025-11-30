using FluentResults;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class TiglRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<TiglRepository> logger)
    : ITiglRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<TiglRepository> _logger = logger;

    public async Task<List<MatchReport>> GetAllMatchReports(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var matchReports = await dbContext.GameReports
            .OrderByDescending(mr => mr.EndTimestamp)
            .ToListAsync(cancellationToken);

        return matchReports;
    }

    public async Task<MatchReport?> GetMatchReportByGameId(string gameId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var matchReport = await dbContext.GameReports
            .FirstOrDefaultAsync(x => x.GameId == gameId, cancellationToken);

        return matchReport;
    }

    public async Task<MatchReport?> GetMatchReportById(int gameId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var matchReport = await dbContext.GameReports
            .Include(mr => mr.PlayerResults)
            .FirstOrDefaultAsync(x => x.Id == gameId, cancellationToken);

        return matchReport;
    }

    public async Task<MatchReport?> GetMatchReportWithPlayerResults(int gameId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var matchReport = await dbContext.GameReports
            .Include(mr => mr.PlayerResults)
            .Include(mr => mr.PlayerMatchAsyncStats)
            .Include(mr => mr.PlayerMatchGlickoStats)
            .Include(mr => mr.PlayerMatchTrueSkillStats)
            .FirstOrDefaultAsync(x => x.Id == gameId, cancellationToken);

        return matchReport;
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

    public async Task<List<TiglParameter>> GetAllTiglParameters(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.TiglParameters
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<bool>> UpdateTiglParameter(TiglParameterName parameterName, bool enabled, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var parameter = await dbContext.TiglParameters.FirstOrDefaultAsync(x => x.Name == parameterName, cancellationToken);
        if (parameter is null)
        {
            _logger.LogWarning("TIGL parameter {Parameter} not found", parameterName);
            return Result.Fail<bool>("Parameter not found");
        }

        parameter.Enabled = enabled;

        try
        {
            dbContext.TiglParameters.Update(parameter);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update TIGL parameter {Parameter}", parameterName);
            return Result.Fail<bool>("Failed to update parameter");
        }
    }
}
