using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record ActionCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    ActionCardName ActionCardName,
    TimingWindow ActionCardWindow)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
