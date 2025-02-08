using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncOpponentsStatsQuery(int limit) : IRequest<AsyncOpponentsSummaryStatsDto>
{
    public int Limit { get; set; } = limit;
}
