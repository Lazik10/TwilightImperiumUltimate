using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

public record SystemTileDto(
    int Id,
    SystemTileName SystemTileName,
    string SystemTileCode,
    SystemTileCategory SystemTileCategory,
    int Resources,
    int Influence,
    bool HasPlanets,
    FactionName FactionName,
    IReadOnlyCollection<PlanetDto> Planets,
    IReadOnlyCollection<WormholeDto> Wormholes,
    AnomalyName AnomalyName,
    GameVersion GameVersion);
