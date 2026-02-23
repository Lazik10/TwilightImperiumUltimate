using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class FlagshipCard : BaseCard<FlagshipName>
{
    public override CardType Type { get; set; } = CardType.Flagship;

    public override FlagshipName EnumName { get; set; }

    public FactionName FactionName { get; set; }
}
