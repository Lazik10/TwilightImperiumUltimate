using FluentResults;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ITiglRepository
{
    Task<List<MatchReport>> GetAllMatchReports(CancellationToken cancellationToken);

    Task<MatchReport?> GetMatchReportByGameId(string gameId, CancellationToken cancellationToken);

    Task<MatchReport?> GetMatchReportById(int gameId, CancellationToken cancellationToken);

    Task<MatchReport?> GetMatchReportWithPlayerResults(int gameId, CancellationToken cancellationToken);

    Task<MatchReport> InsertNewGameReport(MatchReport matchReport, CancellationToken cancellationToken);

    Task<Result<bool>> UpdateMatch(int matchId, CancellationToken cancellationToken);

    Task<TiglParameter?> GetTiglParameter(TiglParameterName parameterName, CancellationToken cancellationToken);

    Task<List<TiglParameter>> GetAllTiglParameters(CancellationToken cancellationToken);

    Task<Result<bool>> UpdateTiglParameter(TiglParameterName parameterName, bool enabled, CancellationToken cancellationToken);
}
