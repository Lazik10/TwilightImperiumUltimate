using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.Users;

public sealed class LinkDiscordPlayerProfilesCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<LinkDiscordPlayerProfilesCommand, bool>
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task<bool> Handle(LinkDiscordPlayerProfilesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _userRepository.UpdateLinkedPlayerProfiles(
            request.UserId,
            request.DiscordUsername,
            request.AsyncPlayerProfileId,
            request.TiglPlayerProfileId,
            cancellationToken);
    }
}
