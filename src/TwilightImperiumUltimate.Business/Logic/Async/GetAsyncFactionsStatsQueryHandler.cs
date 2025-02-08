using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncFactionsStatsQueryHandler(
    IAsyncFactionStatsFactory asyncFactionsStatsFactory)
    : IRequestHandler<GetAsyncFactionsStatsQuery, AsyncFactionsSummaryStatsDto>
{
    private readonly IAsyncFactionStatsFactory _asyncFactionsStatsFactory = asyncFactionsStatsFactory;

    public async Task<AsyncFactionsSummaryStatsDto> Handle(GetAsyncFactionsStatsQuery request, CancellationToken cancellationToken)
    {
        return await _asyncFactionsStatsFactory.CreateAsyncFactionStatsSummary(cancellationToken);
    }
}
