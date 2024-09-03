namespace TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

public record SliceDraftDto(
    int Id,
    string Name,
    string EventName,
    string Description,
    string SliceNames,
    int SliceCount,
    string SliceDraftString,
    string SliceDraftGeneratorLink,
    string SliceDraftArchiveLink,
    string UserName,
    string UserId,
    float Rating,
    int NumberOfVotes);