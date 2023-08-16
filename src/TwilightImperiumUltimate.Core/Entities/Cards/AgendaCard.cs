using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class AgendaCard : BaseCard
{
    public static CardType CardType => CardType.Agenda;

    public AgendaCardName AgendaCardName { get; set; }

    public AgendaCardType AgendaCardType { get; set; }
}
