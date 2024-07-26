using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record StrategyCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    StrategyCardName StrategyCardName,
    InitiativeOrder InitiativeOrder,
    string PrimaryAbilityText,
    string SecondaryAbilityText)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
