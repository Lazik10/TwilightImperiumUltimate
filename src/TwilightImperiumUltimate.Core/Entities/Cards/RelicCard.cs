using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class RelicCard : BaseCard<RelicCardName>
{
    public override CardType Type { get; set; } = CardType.Relic;

    public override RelicCardName EnumName { get; set; }
}
