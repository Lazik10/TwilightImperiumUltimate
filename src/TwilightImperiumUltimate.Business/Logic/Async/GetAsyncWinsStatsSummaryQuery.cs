using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncWinsStatsSummaryQuery(int limit) : IRequest<AsyncWinsSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
