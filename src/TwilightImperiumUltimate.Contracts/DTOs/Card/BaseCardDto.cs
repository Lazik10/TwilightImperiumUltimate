using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record BaseCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion)
    : ICardDto;
