using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class FrontierCard : BaseCard<FrontierCardName>
{
    public override CardType Type { get; set; } = CardType.Frontier;

    public override FrontierCardName EnumName { get; set; }
}
