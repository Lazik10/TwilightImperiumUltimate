using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncCombatStatsSummaryQuery(int limit) : IRequest<AsyncCombatSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
