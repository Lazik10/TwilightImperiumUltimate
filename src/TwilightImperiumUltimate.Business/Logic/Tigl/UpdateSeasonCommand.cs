using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class UpdateSeasonCommand(int seasonNumber, string seasonName, DateOnly startDate, DateOnly endDate)
    : IRequest<UpdateSeasonResponse>
{
    public int SeasonNumber { get; set; } = seasonNumber;

    public string SeasonName { get; set; } = seasonName;

    public DateOnly StartDate { get; set; } = startDate;

    public DateOnly EndDate { get; set; } = endDate;
}
