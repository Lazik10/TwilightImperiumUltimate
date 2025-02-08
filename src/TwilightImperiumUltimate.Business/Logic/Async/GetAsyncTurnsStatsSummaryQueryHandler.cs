using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncTurnsStatsSummaryQueryHandler(
    IAsyncTurnsStatsFactory asyncTurnsStatsFactory)
    : IRequestHandler<GetAsyncTurnsStatsSummaryQuery, AsyncTurnsSummaryStatsDto>
{
    private readonly IAsyncTurnsStatsFactory _asyncTurnsStatsFactory = asyncTurnsStatsFactory;

    public async Task<AsyncTurnsSummaryStatsDto> Handle(GetAsyncTurnsStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncTurnsStatsFactory.CreateAsyncTurnsStatsSummary(request.Limit, cancellationToken);
    }
}
