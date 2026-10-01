using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncDurationsStatsSummaryQueryHandler(
    IAsyncStatisticsSnapshotReader snapshotReader)
    : IRequestHandler<GetAsyncDurationsStatsSummaryQuery, AsyncDurationsSummaryStatsDto>
{
    private readonly IAsyncStatisticsSnapshotReader _snapshotReader = snapshotReader;

    public async Task<AsyncDurationsSummaryStatsDto> Handle(GetAsyncDurationsStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _snapshotReader.GetDurationsAsync(request.Limit, cancellationToken);
    }
}
