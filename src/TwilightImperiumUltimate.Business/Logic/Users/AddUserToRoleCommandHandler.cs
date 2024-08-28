namespace TwilightImperiumUltimate.Business.Logic.Users;

public class AddUserToRoleCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<AddUserToRoleCommand, bool>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<bool> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        return await _userRepository.AddUserToRole(request.UserId, request.RoleName);
    }
}
