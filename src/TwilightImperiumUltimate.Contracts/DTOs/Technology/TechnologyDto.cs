using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Technology;

public record TechnologyDto(
    int Id,
    TechnologyName TechnologyName,
    TechnologyType Type,
    TechnologyLevel Level,
    bool IsFactionTechnology,
    FactionName FactionName,
    string Name,
    string Text,
    GameVersion GameVersion);
