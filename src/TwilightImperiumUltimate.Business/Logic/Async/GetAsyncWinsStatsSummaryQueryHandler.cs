using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncWinsStatsSummaryQueryHandler(
    IAsyncWinsStatsFactory asyncWinsStatsFactory)
    : IRequestHandler<GetAsyncWinsStatsSummaryQuery, AsyncWinsSummaryStatsDto>
{
    private readonly IAsyncWinsStatsFactory _asyncWinsStatsFactory = asyncWinsStatsFactory;

    public async Task<AsyncWinsSummaryStatsDto> Handle(GetAsyncWinsStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncWinsStatsFactory.CreateAsyncWinsStatsSummary(request.Limit, cancellationToken);
    }
}
