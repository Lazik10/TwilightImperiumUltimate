using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class FrontierCard : BaseCard
{
    public static CardType CardType => CardType.Frontier;

    public FrontierCardName FrontierCardName { get; set; }
}
