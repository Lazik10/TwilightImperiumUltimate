using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class SpecialComponentCard : BaseCard<SpecialComponentName>
{
    public override CardType Type { get; set; } = CardType.SpecialComponent;

    public override SpecialComponentName EnumName { get; set; }

    public FactionName FactionName { get; set; }

    public int Count { get; set; }

    public SpecialComponentType SpecialType { get; set; }
}
