namespace TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

public record AsyncPlayerProfileListDto(IReadOnlyCollection<AsyncPlayerProfileDto> PlayerProfiles);
