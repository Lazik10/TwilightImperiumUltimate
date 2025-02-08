using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncDurationsStatsSummaryQueryHandler(
    IAsyncDurationsStatsFactory asyncCombatStatsFactory)
    : IRequestHandler<GetAsyncDurationsStatsSummaryQuery, AsyncDurationsSummaryStatsDto>
{
    private readonly IAsyncDurationsStatsFactory _asyncDurationsStatsFactory = asyncCombatStatsFactory;

    public async Task<AsyncDurationsSummaryStatsDto> Handle(GetAsyncDurationsStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncDurationsStatsFactory.CreateAsyncDurationsStatsSummary(request.Limit, cancellationToken);
    }
}
