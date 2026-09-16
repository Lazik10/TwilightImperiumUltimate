using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncGeneralStatsSummaryQueryHandler(
    IAsyncStatisticsSnapshotReader snapshotReader)
    : IRequestHandler<GetAsyncGeneralStatsSummaryQuery, AsyncGeneralSummaryStatsDto>
{
    private readonly IAsyncStatisticsSnapshotReader _snapshotReader = snapshotReader;

    public async Task<AsyncGeneralSummaryStatsDto> Handle(GetAsyncGeneralStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _snapshotReader.GetGeneralAsync(cancellationToken);
    }
}
