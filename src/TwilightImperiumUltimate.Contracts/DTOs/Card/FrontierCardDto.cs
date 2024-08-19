using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record FrontierCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    FrontierCardName FrontierCardName)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
