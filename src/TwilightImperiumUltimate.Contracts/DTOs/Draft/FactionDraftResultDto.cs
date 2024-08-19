using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Draft;

public record FactionDraftResultDto(IReadOnlyDictionary<int, List<FactionName>> FactionDraftResults);
