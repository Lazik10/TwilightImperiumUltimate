using FluentResults;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IRankRepository
{
    Task<Result<int>> AddRank(int tiglUserId, TiglRankName rankName, TiglLeague league, long timestamp, int matchId, CancellationToken cancellationToken);

    Task<Result<TiglRank>> GetCurrentRank(int tiglUserId, TiglLeague league, CancellationToken cancellationToken);

    Task<Result<TiglRankName>> GetPreMatchRank(int tiglUserId, long matchStart, TiglLeague league, CancellationToken cancellationToken);

    Task<Result<bool>> UpdateUserAndMatchStats(MatchReport matchReport, TiglUser player, AsyncPlayerMatchStats asyncPlayerMatchStats, GlickoPlayerMatchStats glickoPlayerMatchStats, TrueSkillPlayerMatchStats trueSkillMatchStats, CancellationToken cancellationToken);
}
