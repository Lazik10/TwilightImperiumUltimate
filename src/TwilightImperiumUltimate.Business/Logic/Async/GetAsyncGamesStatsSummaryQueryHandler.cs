using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncGamesStatsSummaryQueryHandler(
    IAsyncGamesStatsFactory asyncGamesStatsFactory)
    : IRequestHandler<GetAsyncGamesStatsSummaryQuery, AsyncGamesSummaryStatsDto>
{
    private readonly IAsyncGamesStatsFactory _asyncGamesStatsFactory = asyncGamesStatsFactory;

    public async Task<AsyncGamesSummaryStatsDto> Handle(GetAsyncGamesStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _asyncGamesStatsFactory.CreateAsyncGamesStatsSummary(request.Limit, cancellationToken);
    }
}
