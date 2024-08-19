using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class ObjectiveCard : BaseCard<ObjectiveCardName>
{
    public override CardType Type { get; set; } = CardType.Objective;

    public override ObjectiveCardName EnumName { get; set; }

    public ObjectiveCardType ObjectiveCardType { get; set; }

    public TimingWindow TimingWindow { get; set; }
}
