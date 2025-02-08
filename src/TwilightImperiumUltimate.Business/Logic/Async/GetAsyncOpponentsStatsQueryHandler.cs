using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncOpponentsStatsQueryHandler(
    IAsyncOpponentsStatsFactory asyncOpponentsStatsFactory)
    : IRequestHandler<GetAsyncOpponentsStatsQuery, AsyncOpponentsSummaryStatsDto>
{
    private readonly IAsyncOpponentsStatsFactory _asyncOpponentsStatsFactory = asyncOpponentsStatsFactory;

    public async Task<AsyncOpponentsSummaryStatsDto> Handle(GetAsyncOpponentsStatsQuery request, CancellationToken cancellationToken)
    {
        return await _asyncOpponentsStatsFactory.CreateAsyncOpponentsStatsSummary(request.Limit, cancellationToken);
    }
}
