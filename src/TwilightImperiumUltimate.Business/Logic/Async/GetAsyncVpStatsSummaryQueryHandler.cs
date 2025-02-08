using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncVpStatsSummaryQueryHandler(
    IAsyncVpStatsFactory asyncVpStatsFactory)
    : IRequestHandler<GetAsyncVpStatsSummaryQuery, AsyncVpSummaryStatsDto>
{
    private readonly IAsyncVpStatsFactory _asyncVpStatsFactory = asyncVpStatsFactory;

    public async Task<AsyncVpSummaryStatsDto> Handle(GetAsyncVpStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncVpStatsFactory.CreateAsyncVpStatsSummary(request.Limit, cancellationToken);
    }
}
