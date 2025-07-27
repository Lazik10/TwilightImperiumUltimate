using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class RegisterNewTiglUserCommandHandler(
    ITiglRepository tiglRepository)
    : IRequestHandler<RegisterNewTiglUserCommand, Result<int>>
{
    public async Task<Result<int>> Handle(RegisterNewTiglUserCommand request, CancellationToken cancellationToken)
    {
        if (request.DiscordId <= 0)
            return Result.Fail<int>("Invalid DiscordId.");

        var result = await tiglRepository.RegisterNewTiglUser(
            request.DiscordId,
            request.TiglUserName,
            cancellationToken,
            discordTag: request.DiscordTag);

        return result;
    }
}
