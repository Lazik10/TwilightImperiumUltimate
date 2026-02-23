using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record BreakthroughCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    BreakthroughName BreakthroughName,
    FactionName FactionName)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
