using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class GalacticEventCard : BaseCard<GalacticEventName>
{
    public override CardType Type { get; set; } = CardType.GalacticEvent;

    public override GalacticEventName EnumName { get; set; }
}
