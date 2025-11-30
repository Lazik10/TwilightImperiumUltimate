using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Prestige;

public record AwardPrestigeRankCommand(AwardPrestigeRankRequest Request) : IRequest<AwardPrestigeRankResponse>;
