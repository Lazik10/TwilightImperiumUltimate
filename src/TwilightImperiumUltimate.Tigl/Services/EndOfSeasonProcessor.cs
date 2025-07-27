
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Tigl.Services;

public class EndOfSeasonProcessor(
    ITiglRepository tiglRepository)
    : IEndOfSeasonProcessor
{
    public Task<bool> ProcessEndOfSeason()
    {
        throw new NotImplementedException();
    }
}
