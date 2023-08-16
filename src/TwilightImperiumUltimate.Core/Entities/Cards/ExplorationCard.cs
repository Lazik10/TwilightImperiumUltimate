using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.Core.Enums.Galaxy;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class ExplorationCard : BaseCard
{
    public static CardType CardType => CardType.Exploration;

    public ExplorationCardName ExplorationCardName { get; set; }

    public PlanetTrait ExplorationPlanetTrait { get; set; }
}
