using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncGeneralStatsSummaryQueryHandler(
    IAsyncGeneralStatsFactory asyncGeneralStatsFactory)
    : IRequestHandler<GetAsyncGeneralStatsSummaryQuery, AsyncGeneralSummaryStatsDto>
{
    private readonly IAsyncGeneralStatsFactory _asyncGeneralStatsFactory = asyncGeneralStatsFactory;

    public async Task<AsyncGeneralSummaryStatsDto> Handle(GetAsyncGeneralStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncGeneralStatsFactory.CreateAsyncGeneralStatsSummary(cancellationToken);
    }
}
