using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

internal sealed class GetPlayerIdsByDiscordIdQueryHandler(
    ITiglUserRepository tiglUserRepository,
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<GetPlayerIdsByDiscordIdQuery, PlayerIdsByDiscordIdDto>
{
    public async Task<PlayerIdsByDiscordIdDto> Handle(GetPlayerIdsByDiscordIdQuery request, CancellationToken cancellationToken)
    {
        var tiglUserTask = tiglUserRepository.GetTiglUserByDiscordId(request.DiscordId, cancellationToken);
        var asyncPlayerProfileTask = asyncStatsRepository.GetPlayerIdByDiscordId(request.DiscordId, cancellationToken);

        await Task.WhenAll(tiglUserTask, asyncPlayerProfileTask);
        var tiglUser = await tiglUserTask;
        var asyncPlayerProfile = await asyncPlayerProfileTask;

        return new PlayerIdsByDiscordIdDto
        {
            TiglPlayerId = tiglUser?.Id ?? -1,
            AsyncPlayerProfileId = asyncPlayerProfile?.Id ?? -1,
        };
    }
}
