
using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

internal class GetPlayerIdByDiscordIdQueryHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<GetPlayerIdByDiscordIdQuery, AsyncPlayerProfileDto>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncPlayerProfileDto> Handle(GetPlayerIdByDiscordIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _asyncStatsRepository.GetPlayerIdByDiscordId(request.DiscordId, cancellationToken);
        if (result is not null)
        {
            return new AsyncPlayerProfileDto(result.Id, result.DiscordUserName);
        }

        return new AsyncPlayerProfileDto(-1, string.Empty);
    }
}
