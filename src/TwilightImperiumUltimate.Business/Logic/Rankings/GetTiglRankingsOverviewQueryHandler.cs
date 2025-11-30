using MediatR;
using TwilightImperiumUltimate.Contracts.DTOs;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetTiglRankingsOverviewQueryHandler(IRankingsRepository rankingsRepository)
    : IRequestHandler<GetTiglRankingsOverviewQuery, ItemListDto<RankingsUserDto>>
{
    private readonly IRankingsRepository _repo = rankingsRepository;

    public async Task<ItemListDto<RankingsUserDto>> Handle(GetTiglRankingsOverviewQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rows = await _repo.GetUsersRankingsOverview(cancellationToken);

        var grouped = rows
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
            .OrderBy(u => u.TiglUserName)
            .ToList();

        return new ItemListDto<RankingsUserDto>(grouped);
    }
}
