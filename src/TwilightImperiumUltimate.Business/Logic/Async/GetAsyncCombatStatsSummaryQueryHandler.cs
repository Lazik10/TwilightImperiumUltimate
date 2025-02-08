using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncCombatStatsSummaryQueryHandler(
    IAsyncCombatStatsFactory asyncCombatStatsFactory)
    : IRequestHandler<GetAsyncCombatStatsSummaryQuery, AsyncCombatSummaryStatsDto>
{
    private readonly IAsyncCombatStatsFactory _asyncCombatStatsFactory = asyncCombatStatsFactory;

    public async Task<AsyncCombatSummaryStatsDto> Handle(GetAsyncCombatStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncCombatStatsFactory.CreateAsyncCombatStatsSummary(request.Limit, cancellationToken);
    }
}
