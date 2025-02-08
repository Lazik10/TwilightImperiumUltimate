using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncHistoryStatsQueryHandler(
    IAsyncHistoryStatsFactory asyncHistoryStatsFactory)
    : IRequestHandler<GetAsyncHistoryStatsQuery, AsyncHistorySummaryStatsDto>
{
    private readonly IAsyncHistoryStatsFactory _asyncHistoryStatsFactory = asyncHistoryStatsFactory;

    public async Task<AsyncHistorySummaryStatsDto> Handle(GetAsyncHistoryStatsQuery request, CancellationToken cancellationToken)
    {
        return await _asyncHistoryStatsFactory.CreateAsyncHistoryStatsSummary(cancellationToken);
    }
}
