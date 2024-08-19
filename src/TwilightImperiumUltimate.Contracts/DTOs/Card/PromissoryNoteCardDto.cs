using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Card;

public record PromissoryNoteCardDto(
    int Id,
    string Name,
    string Text,
    CardType CardType,
    GameVersion GameVersion,
    PromissoryNoteCardName PromissoryNoteCardName,
    FactionName FactionName)
    : BaseCardDto(Id, Name, Text, CardType, GameVersion);
