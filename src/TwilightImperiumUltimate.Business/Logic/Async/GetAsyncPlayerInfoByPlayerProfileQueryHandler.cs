using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncPlayerInfoByPlayerProfileQueryHandler(
    IAsyncStatsRepository asyncStatsRepository,
    IAsyncPlayerAllStatsFactory asyncPlayerAllStatsFactory)
    : IRequestHandler<GetAsyncPlayerInfoByPlayerProfileQuery, AsyncPlayerProfileSummaryStatsDto>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;
    private readonly IAsyncPlayerAllStatsFactory _asyncPlayerAllStatsFactory = asyncPlayerAllStatsFactory;

    public async Task<AsyncPlayerProfileSummaryStatsDto> Handle(GetAsyncPlayerInfoByPlayerProfileQuery request, CancellationToken cancellationToken)
    {
        var asyncPlayerProfile = await _asyncStatsRepository.GetAsyncPlayerProfileByPlayerRequest(request.DiscordId, request.Name, request.PlayerId, cancellationToken);
        if (asyncPlayerProfile is null)
        {
            return await _asyncPlayerAllStatsFactory.CreateDefaultAsyncPlayerStats();
        }

        return await _asyncPlayerAllStatsFactory.CreateAsyncPlayerStats(asyncPlayerProfile);
    }
}