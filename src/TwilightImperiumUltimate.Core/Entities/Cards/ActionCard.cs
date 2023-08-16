using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class ActionCard : BaseCard
{
    public static CardType CardType => CardType.Action;

    public ActionCardName ActionCardName { get; set; }

    public ActionCardWindow ActionCardWindow { get; set; }
}
