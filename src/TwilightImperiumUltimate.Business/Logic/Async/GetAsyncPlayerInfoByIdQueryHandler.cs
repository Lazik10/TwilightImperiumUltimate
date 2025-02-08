using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncPlayerInfoByIdQueryHandler(
    IAsyncStatsRepository asyncStatsRepository,
    IAsyncPlayerAllStatsFactory asyncPlayerAllStatsFactory)
    : IRequestHandler<GetAsyncPlayerInfoByIdQuery, AsyncPlayerProfileSummaryStatsDto>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;
    private readonly IAsyncPlayerAllStatsFactory _asyncPlayerAllStatsFactory = asyncPlayerAllStatsFactory;

    public async Task<AsyncPlayerProfileSummaryStatsDto> Handle(GetAsyncPlayerInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var asyncPlayerProfile = await _asyncStatsRepository.GetAsyncPlayerProfileByDiscordId(request.DiscordUserId, cancellationToken);
        if (asyncPlayerProfile is null)
        {
            return await _asyncPlayerAllStatsFactory.CreateDefaultAsyncPlayerStats();
        }

        return await _asyncPlayerAllStatsFactory.CreateAsyncPlayerStats(asyncPlayerProfile);
    }
}
