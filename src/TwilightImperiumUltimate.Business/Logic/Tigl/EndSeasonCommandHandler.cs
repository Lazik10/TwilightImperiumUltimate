using FluentResults;
using TwilightImperiumUltimate.Tigl.Services;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class EndSeasonCommandHandler(
    IEndOfSeasonProcessor endOfSeasonProcessor)
    : IRequestHandler<EndSeasonCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(EndSeasonCommand request, CancellationToken cancellationToken)
    {
        var result = await endOfSeasonProcessor.ProcessEndOfSeason(cancellationToken);
        return result;
    }
}
