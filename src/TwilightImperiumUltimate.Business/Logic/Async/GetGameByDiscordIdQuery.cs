using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetGameByDiscordIdQuery(string gameDiscordId) : IRequest<AsyncGameDto>
{
    public string GameDiscordId { get; set; } = gameDiscordId;
}
