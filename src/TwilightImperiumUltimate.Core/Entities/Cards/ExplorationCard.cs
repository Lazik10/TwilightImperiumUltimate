using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class ExplorationCard : BaseCard<ExplorationCardName>
{
    public override CardType Type { get; set; } = CardType.Exploration;

    public override ExplorationCardName EnumName { get; set; }

    public PlanetTrait ExplorationPlanetTrait { get; set; }

    public new GameVersion GameVersion { get; set; } = GameVersion.ProphecyOfKings;
}
