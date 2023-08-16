using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class ObjectiveCard : BaseCard
{
    public static CardType CardType => CardType.Objective;

    public ObjectiveCardName ObjectiveCardName { get; set; }

    public ObjectiveCardType ObjectiveCardType { get; set; }
}
