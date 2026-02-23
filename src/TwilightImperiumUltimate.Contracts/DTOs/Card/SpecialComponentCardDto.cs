using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record SpecialComponentCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    SpecialComponentName SpecialComponentName,
    FactionName FactionName,
    int Count,
    SpecialComponentType SpecialType)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
