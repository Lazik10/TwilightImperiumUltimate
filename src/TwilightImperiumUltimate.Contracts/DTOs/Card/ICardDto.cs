using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public interface ICardDto
{
    int Id { get; init; }

    string Name { get; init; }

    string Text { get; init; }

    CardType CardType { get; init; }

    GameVersion GameVersion { get; init; }
}
