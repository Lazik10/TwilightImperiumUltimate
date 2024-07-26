using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class PromissoryNoteCard : BaseCard<PromissoryNoteCardName>
{
    public override CardType Type { get; set; } = CardType.Promissary;

    public override PromissoryNoteCardName EnumName { get; set; }

    public FactionName Faction { get; set; }
}
