using TwilightImperiumUltimate.Contracts.DTOs.Technology;
using TwilightImperiumUltimate.Contracts.DTOs.Unit;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Faction;

public record FactionDto(
    int Id,
    FactionName FactionName,
    SystemTileName HomeSystem,
    int Commodities,
    ComplexityRating ComplexityRating,
    string Action,
    string History,
    string Quote,
    string SystemStats,
    string SystemInfo,
    GameVersion GameVersion,
    IReadOnlyCollection<UnitWithCountDto> StartingUnits,
    IReadOnlyCollection<TechnologyDto> StartingTechnologies,
    IReadOnlyCollection<PromissoryNoteCardName> PromissaryNotes,
    IReadOnlyCollection<BreakthroughName> Breakthroughs);
