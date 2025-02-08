using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGamesByYearAndMonthQuery(
    int year, int month)
    : IRequest<List<AsyncGameDto>>
{
    public int Year { get; set; } = year;

    public int Month { get; set; } = month;
}
