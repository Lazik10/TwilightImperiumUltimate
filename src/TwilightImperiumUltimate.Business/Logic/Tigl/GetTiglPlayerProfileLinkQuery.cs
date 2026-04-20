using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetTiglPlayerProfileLinkQuery(long DiscordUserId) : IRequest<TiglPlayerProfileLinkDto>;
