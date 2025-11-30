using TwilightImperiumUltimate.DataAccess.DTOs;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IRankingsRepository
{
    Task<List<RankingsRow>> GetUsersRankingsOverview(CancellationToken cancellationToken);

    Task<List<RankingsLeaderDto>> GetLeadersOverview(CancellationToken cancellationToken);
}
