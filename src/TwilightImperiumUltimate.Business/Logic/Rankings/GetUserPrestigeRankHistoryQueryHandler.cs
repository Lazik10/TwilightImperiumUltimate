using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetUserPrestigeRankHistoryQueryHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<GetUserPrestigeRankHistoryQuery, ItemListDto<PrestigeRankHistoryDto>>
{
    public async Task<ItemListDto<PrestigeRankHistoryDto>> Handle(GetUserPrestigeRankHistoryQuery request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var rows = await db.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .Where(x => x.TiglUserId == request.TiglUserId)
            .OrderByDescending(x => x.AchievedAt)
            .Select(x => new PrestigeRankHistoryDto
            {
                League = x.PrestigeRank.League,
                PrestigeRank = x.PrestigeRank.Name,
                Faction = x.PrestigeRank.FactionName,
                Level = x.Rank,
                AchievedAt = x.AchievedAt,
            })
            .ToListAsync(cancellationToken);

        return new ItemListDto<PrestigeRankHistoryDto>(rows);
    }
}
