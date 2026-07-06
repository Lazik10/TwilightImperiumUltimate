namespace TwilightImperiumUltimate.Business.Logic.Users;

public class RemoveRoleFromUserCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<RemoveRoleFromUserCommand, bool>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<bool> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _userRepository.DeleteUserFromRole(request.UserId, request.RoleName);
    }
}
