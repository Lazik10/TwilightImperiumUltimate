using TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class AddRankHistoryCommandHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<AddRankHistoryCommand, AddRankHistoryResponse>
{
    public async Task<AddRankHistoryResponse> Handle(AddRankHistoryCommand request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = new TiglRank
        {
            TiglUserId = request.TiglUserId,
            League = request.League,
            Name = request.Rank,
            AchievedAt = request.AchievedAt,
        };

        db.Ranks.Add(entity);
        await db.SaveChangesAsync(cancellationToken);

        return new AddRankHistoryResponse { Success = true, NewRankHistoryId = entity.Id };
    }
}
