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
    bool IsTigl)
{
    public double DurationHours
    {
        get
        {
            var endTimestamp = EndDate == 0
                ? DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                : EndDate;

            return Math.Max(0, endTimestamp - StartDate) / 3600d;
        }
    }
}
