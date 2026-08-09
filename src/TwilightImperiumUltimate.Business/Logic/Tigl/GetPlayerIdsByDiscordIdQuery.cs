using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public sealed class GetPlayerIdsByDiscordIdQuery(long discordId) : IRequest<PlayerIdsByDiscordIdDto>
{
    public long DiscordId { get; } = discordId;
}
