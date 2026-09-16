using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncFactionsStatsQueryHandler(
    IAsyncStatisticsSnapshotReader snapshotReader)
    : IRequestHandler<GetAsyncFactionsStatsQuery, AsyncFactionsSummaryStatsDto>
{
    private readonly IAsyncStatisticsSnapshotReader _snapshotReader = snapshotReader;

    public async Task<AsyncFactionsSummaryStatsDto> Handle(GetAsyncFactionsStatsQuery request, CancellationToken cancellationToken)
    {
        return await _snapshotReader.GetFactionsAsync(cancellationToken);
    }
}
