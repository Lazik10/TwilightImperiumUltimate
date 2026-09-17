namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncGamesHistoryDto(int Year, int Month, int Count, int New, int Ended)
{
    public int Year { get; set; } = Year;

    public int Month { get; set; } = Month;

    public int Count { get; set; } = Count;

    public int New { get; set; } = New;

    public int Ended { get; set; } = Ended;
}
