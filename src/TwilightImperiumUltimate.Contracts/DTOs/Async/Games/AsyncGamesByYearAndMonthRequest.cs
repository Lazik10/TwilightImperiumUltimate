namespace TwilightImperiumUltimate.Contracts.DTOs.Async.Games;

public class AsyncGamesByYearAndMonthRequest(int year, int month)
{
    public int Year { get; set; } = year;

    public int Month { get; set; } = month;
}