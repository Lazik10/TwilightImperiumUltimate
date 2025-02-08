using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetPlayerIdByDiscordIdQuery(long discordId) : IRequest<AsyncPlayerProfileDto>
{
    public long DiscordId { get; set; } = discordId;
}
