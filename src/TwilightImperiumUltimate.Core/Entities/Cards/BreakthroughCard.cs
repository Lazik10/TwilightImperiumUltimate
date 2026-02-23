using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class BreakthroughCard : BaseCard<BreakthroughName>
{
    public override CardType Type { get; set; } = CardType.Breakthrough;

    public override BreakthroughName EnumName { get; set; }

    public FactionName FactionName { get; set; }

    public new GameVersion GameVersion { get; set; } = GameVersion.ThundersEdge;
}
