using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ITiglUserRepository
{
    Task<TiglUser?> GetTiglUserById(int id, TiglLeague league, CancellationToken cancellationToken);

    Task<TiglUser?> GetTiglUserByDiscordId(long discordId, CancellationToken cancellationToken);

    Task<HashSet<int>> GetAllTiglUserIds(CancellationToken cancellationToken);

    Task<HashSet<int>> GetActiveUserIds(int season, TiglLeague league, CancellationToken cancellationToken);

    Task<List<TiglUser>> GetUsersFromGameReport(IGameReport gameReport, CancellationToken cancellationToken);

    Task<List<TiglUser>> GetUsersByIds(IReadOnlyCollection<int> tiglUserIds, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TiglUser?>> GetTiglUsersBaseInfoById(IReadOnlyCollection<int> tiglUserIds, CancellationToken cancellationToken);

    Task<TiglUser?> FindTiglUserFromGameReportPlayerResult(IPlayerResult playerResult, CancellationToken cancellationToken);

    Task<Result<int>> RegisterNewTiglUser(long discordId, string tiglUserName, long startGameTimestamp, CancellationToken cancellationToken, string discordTag = "", string ttsUserName = "", string ttpgUserName = "", string bgaUserName = "");

    Task<Result<bool>> ChangeTiglUserName(long discordId, string newTiglUserName, CancellationToken cancellationToken);

    Task<List<MatchReport>> GetTiglUserMatchReports(int tiglUserId, long endTimestamp, CancellationToken cancellationToken, bool onlyWins = false, HashSet<TiglFactionName>? factions = null, int count = int.MaxValue);

    Task UpdateTiglPlayers(IReadOnlyCollection<TiglUser> players, CancellationToken cancellationToken);

    Task InsertPlayerMatchStats(IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, CancellationToken cancellationToken);

    Task InsertPlayerMatchStats(IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, CancellationToken cancellationToken);

    Task InsertPlayerMatchStats(IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, CancellationToken cancellationToken);
}