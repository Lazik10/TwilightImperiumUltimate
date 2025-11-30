using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

internal class UpdateSeasonCommandHandler(ISeasonRepository repository)
    : IRequestHandler<UpdateSeasonCommand, UpdateSeasonResponse>
{
    public async Task<UpdateSeasonResponse> Handle(UpdateSeasonCommand request, CancellationToken cancellationToken)
    {
        var season = await repository.GetSeasonByNumber(request.SeasonNumber, cancellationToken);
        if (season is null)
        {
            return new UpdateSeasonResponse
            {
                Success = false,
                ErrorTitle = "Season not found.",
                ErrorMessage = $"Season {request.SeasonNumber} not found",
            };
        }

        season.Name = request.SeasonName;
        season.StartDate = request.StartDate;
        season.EndDate = request.EndDate;

        var success = await repository.UpdateSeason(season, cancellationToken);
        if (!success)
        {
            return new UpdateSeasonResponse
            {
                Success = false,
                ErrorTitle = "Update failed.",
                ErrorMessage = $"Failed to update season {request.SeasonNumber}",
            };
        }

        return new UpdateSeasonResponse
        {
            Success = true,
            SeasonNumber = season.SeasonNumber,
        };
    }
}
