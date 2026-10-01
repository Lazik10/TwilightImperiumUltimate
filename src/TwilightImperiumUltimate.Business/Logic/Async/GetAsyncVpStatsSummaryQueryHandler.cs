using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncVpStatsSummaryQueryHandler(
    IAsyncStatisticsSnapshotReader snapshotReader)
    : IRequestHandler<GetAsyncVpStatsSummaryQuery, AsyncVpSummaryStatsDto>
{
    private readonly IAsyncStatisticsSnapshotReader _snapshotReader = snapshotReader;

    public async Task<AsyncVpSummaryStatsDto> Handle(GetAsyncVpStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _snapshotReader.GetVictoryPointsAsync(request.Limit, cancellationToken);
    }
}
