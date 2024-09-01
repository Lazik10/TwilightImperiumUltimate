using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

public record MapDto(
    int Id,
    string Name,
    string EventName,
    string Description,
    MapTemplate MapTemplate,
    string UserName,
    string UserId,
    string TtsString,
    string MapGeneratorLink,
    string MapArchiveLink,
    float Rating,
    int NumberOfVotes);
