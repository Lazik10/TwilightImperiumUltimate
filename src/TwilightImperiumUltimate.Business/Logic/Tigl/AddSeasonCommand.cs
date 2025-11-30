using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class AddSeasonCommand(
    int seasonNumber,
    string seasonName)
    : IRequest<AddSeasonResponse>
{
    public int SeasonNumber { get; set; } = seasonNumber;

    public string SeasonName { get; set; } = seasonName;
}
