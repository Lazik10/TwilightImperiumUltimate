using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Prestige;

public record RemovePrestigeRankCommand(RemovePrestigeRankRequest Request) : IRequest<RemovePrestigeRankResponse>;
