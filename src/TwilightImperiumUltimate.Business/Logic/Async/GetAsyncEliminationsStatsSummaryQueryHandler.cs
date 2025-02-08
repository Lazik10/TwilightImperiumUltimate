using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncEliminationsStatsSummaryQueryHandler(
    IAsyncEliminationsStatsFactory asyncEliminationsStatsFactory)
    : IRequestHandler<GetAsyncEliminationsStatsSummaryQuery, AsyncEliminationsSummaryStatsDto>
{
    private readonly IAsyncEliminationsStatsFactory _asyncEliminationsStatsFactory = asyncEliminationsStatsFactory;

    public async Task<AsyncEliminationsSummaryStatsDto> Handle(GetAsyncEliminationsStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncEliminationsStatsFactory.CreateAsyncEliminationsStatsSummary(request.Limit, cancellationToken);
    }
}
