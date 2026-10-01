using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncTurnsStatsSummaryQueryHandler(
    IAsyncStatisticsSnapshotReader snapshotReader)
    : IRequestHandler<GetAsyncTurnsStatsSummaryQuery, AsyncTurnsSummaryStatsDto>
{
    private readonly IAsyncStatisticsSnapshotReader _snapshotReader = snapshotReader;

    public async Task<AsyncTurnsSummaryStatsDto> Handle(GetAsyncTurnsStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _snapshotReader.GetTurnsAsync(request.Limit, cancellationToken);
    }
}
