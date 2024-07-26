using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class StrategyCard : BaseCard<StrategyCardName>
{
    public override CardType Type { get; set; } = CardType.Strategy;

    public override StrategyCardName EnumName { get; set; }

    public InitiativeOrder InitiativeOrder { get; set; }

    public string PrimaryAbilityText { get; set; } = string.Empty;

    public string SecondaryAbilityText { get; set; } = string.Empty;
}
