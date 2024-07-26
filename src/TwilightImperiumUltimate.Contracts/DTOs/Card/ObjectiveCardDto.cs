using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record ObjectiveCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    ObjectiveCardName ObjectiveCardName,
    ObjectiveCardType ObjectiveCardType,
    TimingWindow TimingWindow)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
