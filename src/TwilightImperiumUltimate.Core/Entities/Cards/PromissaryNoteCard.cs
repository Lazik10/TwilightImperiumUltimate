using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.Core.Enums.Races;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class PromissaryNoteCard : BaseCard
{
    public static CardType CardType => CardType.Promissary;

    public PromissoryNoteName PromissaryNoteCardName { get; set; }

    public FactionName Faction { get; set; }
}
