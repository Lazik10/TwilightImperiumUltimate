using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class SetActiveSeasonCommand(int seasonNumber) : IRequest<SetActiveSeasonResponse>
{
    public int SeasonNumber { get; } = seasonNumber;
}
