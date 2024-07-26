using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class ActionCard : BaseCard<ActionCardName>
{
    public override ActionCardName EnumName { get; set; }

    public override CardType Type { get; set; } = CardType.Action;

    public TimingWindow TimingWindow { get; set; }
}
