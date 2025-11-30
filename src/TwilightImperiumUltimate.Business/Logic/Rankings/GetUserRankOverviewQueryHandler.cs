using MediatR;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetUserRankOverviewQueryHandler(IRankingsRepository rankingsRepository)
    : IRequestHandler<GetUserRankOverviewQuery, RankingsUserDto>
{
    public async Task<RankingsUserDto> Handle(GetUserRankOverviewQuery request, CancellationToken cancellationToken)
    {
        var rows = await rankingsRepository.GetUsersRankingsOverview(cancellationToken);
        var grouped = rows
            .Where(r => r.TiglUserId == request.TiglUserId)
            .GroupBy(r => new { r.TiglUserId, r.TiglUserName })
            .Select(g => new RankingsUserDto
            {
                TiglUserId = g.Key.TiglUserId,
                TiglUserName = g.Key.TiglUserName,
                Leagues = g.OrderBy(x => x.League).Select(x => new RankingsLeagueInfoDto
                {
                    League = x.League,
                    CurrentRank = x.HighestRank,
                    HighestPrestigeRank = x.HighestPrestigeRank,
                    HighestPrestigeRankLevel = x.HighestPrestigeRankLevel,
                    FactionPrestigeRankCount = x.FactionPrestigeRankCount,
                    LastAchievedAt = x.LastAchievedAt,
                    GamesPlayed = x.GamesPlayed,
                }).ToList(),
            })
            .FirstOrDefault();

        return grouped ?? new RankingsUserDto { TiglUserId = request.TiglUserId, TiglUserName = string.Empty };
    }
}
