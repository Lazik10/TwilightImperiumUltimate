using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

public record PlanetDto(
    int Id,
    PlanetName PlanetName,
    int Resources,
    int Influence,
    bool IsLegendary,
    TechnologyType TechnologySkip,
    PlanetTrait PlanetTrait,
    GameVersion GameVersion);
