using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record FlagshipCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    FlagshipName FlagshipName,
    FactionName FactionName)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
