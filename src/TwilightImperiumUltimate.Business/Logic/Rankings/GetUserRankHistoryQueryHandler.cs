using MediatR;
using TwilightImperiumUltimate.Contracts.DTOs;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.DataAccess.DbContexts;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetUserRankHistoryQueryHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<GetUserRankHistoryQuery, ItemListDto<RankHistoryDto>>
{
    public async Task<ItemListDto<RankHistoryDto>> Handle(GetUserRankHistoryQuery request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var rows = await db.Ranks
            .Where(r => r.TiglUserId == request.TiglUserId)
            .OrderByDescending(r => r.AchievedAt)
            .Select(r => new RankHistoryDto
            {
                Id = r.Id,
                League = r.League,
                Rank = r.Name,
                AchievedAt = r.AchievedAt,
            })
            .ToListAsync(cancellationToken);

        return new ItemListDto<RankHistoryDto>(rows);
    }
}
