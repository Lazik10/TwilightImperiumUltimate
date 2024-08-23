using TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

namespace TwilightImperiumUltimate.Contracts.DTOs.SliceGeneration;

public record SliceDto(
    int Id,
    IReadOnlyList<SystemTileDto> SystemTiles);
