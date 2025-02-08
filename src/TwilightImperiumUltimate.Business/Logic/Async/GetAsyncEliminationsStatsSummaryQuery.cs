using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncEliminationsStatsSummaryQuery(int limit) : IRequest<AsyncEliminationsSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
