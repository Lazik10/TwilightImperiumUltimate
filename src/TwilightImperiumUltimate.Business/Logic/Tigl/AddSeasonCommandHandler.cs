using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class AddSeasonCommandHandler(
    ISeasonRepository seasonRepository)
    : IRequestHandler<AddSeasonCommand, AddSeasonResponse>
{
    public async Task<AddSeasonResponse> Handle(AddSeasonCommand request, CancellationToken cancellationToken)
    {
        var result = await seasonRepository.AddNewSeason(request.SeasonNumber, request.SeasonName, cancellationToken);
        if (result.IsFailed)
        {
            return new AddSeasonResponse
            {
                Success = result.IsSuccess,
                ErrorTitle = "Add Season Failed",
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Message)),
            };
        }

        return new AddSeasonResponse{ Success = true, SeasonNumber = result.Value.SeasonNumber, SeasonName = result.Value.Name };
    }
}
