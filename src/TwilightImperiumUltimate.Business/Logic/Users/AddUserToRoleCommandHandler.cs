namespace TwilightImperiumUltimate.Business.Logic.Users;

public class AddUserToRoleCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<AddUserToRoleCommand, bool>
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task<bool> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        => request is null
            ? throw new ArgumentNullException(nameof(request))
            : _userRepository.AddUserToRole(request.UserId, request.RoleName);
}
