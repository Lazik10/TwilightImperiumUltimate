using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncTurnsStatsSummaryQuery(int limit) : IRequest<AsyncTurnsSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
