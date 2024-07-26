using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record AgendaCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    AgendaCardName AgendaCardName,
    AgendaCardType AgendaCardType)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
