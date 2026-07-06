using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.DataAccess.DTOs;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IRankingsRepository
{
    Task<List<RankingsRow>> GetUsersRankingsOverview(CancellationToken cancellationToken);

    Task<List<RankingsLeaderDto>> GetLeadersOverview(CancellationToken cancellationToken);
}
