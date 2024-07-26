using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

public record WormholeDto(
    int Id,
    WormholeName WormholeName,
    SystemTileName SystemTileName,
    GameVersion GameVersion);