using TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class RemoveRankHistoryCommandHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<RemoveRankHistoryCommand, RemoveRankHistoryResponse>
{
    public async Task<RemoveRankHistoryResponse> Handle(RemoveRankHistoryCommand request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = await db.Ranks.FindAsync(new object?[] { request.RankHistoryId }, cancellationToken);
        if (entity == null)
        {
            return new RemoveRankHistoryResponse { Success = false, ErrorTitle = "Not Found", ErrorMessage = "Rank history entry not found." };
        }

        db.Ranks.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);

        return new RemoveRankHistoryResponse { Success = true };
    }
}
