using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.Core.Interfaces;

public interface ICard
{
    string Name { get; }

    string Text { get; }

    CardType Type { get; }
}
