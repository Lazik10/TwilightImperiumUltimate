using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class RelicCard : BaseCard
{
    public static CardType CardType => CardType.Relic;

    public RelicCardName RelicCardName { get; set; }
}
