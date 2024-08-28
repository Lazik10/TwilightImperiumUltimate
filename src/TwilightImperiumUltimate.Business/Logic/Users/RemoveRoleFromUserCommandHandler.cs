namespace TwilightImperiumUltimate.Business.Logic.Users;

public class RemoveRoleFromUserCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<RemoveRoleFromUserCommand, bool>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<bool> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        return await _userRepository.DeleteUserFromRole(request.UserId, request.RoleName);
    }
}
