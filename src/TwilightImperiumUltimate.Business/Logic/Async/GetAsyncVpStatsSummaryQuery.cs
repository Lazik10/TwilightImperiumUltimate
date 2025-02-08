using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncVpStatsSummaryQuery(int limit) : IRequest<AsyncVpSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
