using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGameDatesQueryHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<GetAllAsyncGameDatesQuery, AsyncGameDatesDto>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncGameDatesDto> Handle(GetAllAsyncGameDatesQuery request, CancellationToken cancellationToken)
    {
        var timestamps = await _asyncStatsRepository.GetAllAsyncGameDates(cancellationToken);

        var gameDatesDto = timestamps
             .Select(ts => DateTimeOffset.FromUnixTimeSeconds(ts).UtcDateTime)
             .GroupBy(dt => dt.Year)
             .Select(g => new AsyncGameYearMonthDto(
                 g.Key,
                 g.Select(dt => dt.Month)
                    .Distinct()
                    .OrderByDescending(m => m)
                    .ToList()))
             .OrderByDescending(dto => dto.Year)
             .ToList();

        return new AsyncGameDatesDto(gameDatesDto);
    }
}
