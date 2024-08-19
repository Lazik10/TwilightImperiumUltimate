using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record ExplorationCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    ExplorationCardName ExplorationCardName,
    PlanetTrait PlanetTrait)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
