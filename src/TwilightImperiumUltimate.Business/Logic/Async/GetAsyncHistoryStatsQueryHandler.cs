using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncHistoryStatsQueryHandler(
    IAsyncStatisticsSnapshotReader snapshotReader)
    : IRequestHandler<GetAsyncHistoryStatsQuery, AsyncHistorySummaryStatsDto>
{
    private readonly IAsyncStatisticsSnapshotReader _snapshotReader = snapshotReader;

    public async Task<AsyncHistorySummaryStatsDto> Handle(GetAsyncHistoryStatsQuery request, CancellationToken cancellationToken)
    {
        return await _snapshotReader.GetHistoryAsync(cancellationToken);
    }
}
