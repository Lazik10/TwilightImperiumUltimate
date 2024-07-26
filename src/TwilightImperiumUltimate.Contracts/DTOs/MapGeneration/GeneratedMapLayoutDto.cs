using TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

namespace TwilightImperiumUltimate.Contracts.DTOs.MapGeneration;

public record GeneratedMapLayoutDto(IReadOnlyDictionary<int, SystemTileDto> MapLayout);