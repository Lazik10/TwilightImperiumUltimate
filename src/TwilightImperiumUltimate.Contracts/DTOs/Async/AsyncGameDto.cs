namespace TwilightImperiumUltimate.Contracts.DTOs.Async;

public record AsyncGameDto(
    int Id,
    string AsyncGameID,
    string AsyncFunGameName,
    long StartDate,
    long EndDate,
    bool Finished,
    bool ValidEnd,
    int PlayerCount,
    int Round,
    int Scoreboard,
    bool IsTigl);
