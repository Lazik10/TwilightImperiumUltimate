using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Draft;

public record FactionColorDraftResultDto(IReadOnlyDictionary<FactionName, PlayerColor> FactionColorDraftResults);
