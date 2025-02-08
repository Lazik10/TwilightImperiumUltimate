using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncDurationsStatsSummaryQuery(int limit) : IRequest<AsyncDurationsSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
