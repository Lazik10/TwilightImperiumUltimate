using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class SetActiveSeasonCommandHandler(
    ISeasonRepository tiglRepository,
    IMemoryCache cache)
    : IRequestHandler<SetActiveSeasonCommand, SetActiveSeasonResponse>
{
    private const string CacheKeyPrefix = "season-leaderboard-";

    public async Task<SetActiveSeasonResponse> Handle(SetActiveSeasonCommand request, CancellationToken cancellationToken)
    {
        var result = await tiglRepository.SetActiveSeason(request.SeasonNumber, cancellationToken);
        if (result.IsFailed)
        {
            return new SetActiveSeasonResponse
            {
                Success = false,
                ErrorTitle = "Set Active Season Failed",
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Message)),
            };
        }

        // Invalidate cache for the newly active season so it repopulates on next request
        cache.Remove(CacheKeyPrefix + result.Value.SeasonNumber);

        return new SetActiveSeasonResponse
        {
            Success = result.IsSuccess,
            SeasonNumber = result.Value.SeasonNumber,
        };
    }
}
