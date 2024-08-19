using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class AgendaCard : BaseCard<AgendaCardName>
{
    public override AgendaCardName EnumName { get; set; }

    public override CardType Type { get; set; } = CardType.Agenda;

    public AgendaCardType AgendaCardType { get; set; }
}
