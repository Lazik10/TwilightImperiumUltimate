using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncGamesStatsSummaryQueryHandler(
    IAsyncStatisticsSnapshotReader snapshotReader)
    : IRequestHandler<GetAsyncGamesStatsSummaryQuery, AsyncGamesSummaryStatsDto>
{
    private readonly IAsyncStatisticsSnapshotReader _snapshotReader = snapshotReader;

    public async Task<AsyncGamesSummaryStatsDto> Handle(GetAsyncGamesStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _snapshotReader.GetGamesAsync(request.Limit, cancellationToken);
    }
}
