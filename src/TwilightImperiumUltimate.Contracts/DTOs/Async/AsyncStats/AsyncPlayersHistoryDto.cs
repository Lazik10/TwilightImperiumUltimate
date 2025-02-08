namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncPlayersHistoryDto(int Year, int Month, int Count, int Growth)
{
    public int Year { get; set; } = Year;

    public int Month { get; set; } = Month;

    public int Count { get; set; } = Count;

    public int Growth { get; set; } = Growth;
}
