using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record RelicCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    RelicCardName RelicCardName)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
