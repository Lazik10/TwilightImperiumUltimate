using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncGamesStatsSummaryQuery(int limit) : IRequest<AsyncGamesSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
