using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class StrategyCard : BaseCard
{
    public static CardType CardType => CardType.Strategy;

    public StrategyCardName StrategyCardName { get; set; }

    public InitiativeOrder InitiativeOrder { get; set; }

    public string PrimaryAbilityText { get; set; } = string.Empty;

    public string SecondaryAbilityText { get; set; } = string.Empty;
}
