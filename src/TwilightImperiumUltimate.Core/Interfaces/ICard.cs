using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Interfaces;

public interface ICard<TCardNameEnum>
    where TCardNameEnum : Enum
{
    TCardNameEnum EnumName { get; set; }

    string Name { get; }

    string Text { get; set; }

    CardType Type { get; set; }
}
