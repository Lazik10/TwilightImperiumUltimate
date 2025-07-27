using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ITiglRepository
{
    Task<List<TiglUser>> GetUsersFromGameReport(IGameReport gameReport, CancellationToken cancellationToken);

    Task<MatchReport> InsertNewGameReport(MatchReport matchReport, CancellationToken cancellationToken);

    Task<Result<int>> RegisterNewTiglUser(
        long discordId,
        string tiglUserName,
        CancellationToken cancellationToken,
        string discordTag = "",
        string ttsUserName = "",
        string ttpgUserName = "",
        string bgaUserName = "");

    Task<Result<bool>> ChangeTiglUserName(
        long discordId,
        string newTiglUserName,
        CancellationToken cancellationToken);

    Task<TiglUser?> GetTiglUserByDiscordId(long discordId, CancellationToken cancellationToken);

    Task<TiglUser?> FindTiglUserFromGameReportPlayerResult(IPlayerResult playerResult, CancellationToken cancellationToken);

    Task UpdateTiglPlayers(IReadOnlyCollection<TiglUser> players, CancellationToken cancellationToken);

    Task InsertPlayerMatchStats(IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, CancellationToken cancellationToken);

    Task InsertPlayerMatchStats(IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, CancellationToken cancellationToken);

    Task InsertPlayerMatchStats(IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, CancellationToken cancellationToken);

    Task<int> GetCurrentSeason(CancellationToken cancellationToken);

    Task<bool> CreateNewSeason(CancellationToken cancellationToken);

    Task<Result<bool>> UpdateMatch(int matchId, CancellationToken cancellationToken);

    Task<MatchReport?> GetMatchReport(string gameId, CancellationToken cancellationToken);

    Task<TiglParameter?> GetTiglParameter(TiglParameterName parameterName, CancellationToken cancellationToken);
}
