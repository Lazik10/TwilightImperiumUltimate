using TwilightImperiumUltimate.Contracts.ApiContracts.User;

namespace TwilightImperiumUltimate.Business.Logic.Users;

public class CheckRegistrationInfoQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<CheckRegistrationInfoQuery, UserRegistrationPrecheckResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserRegistrationPrecheckResponse> Handle(CheckRegistrationInfoQuery request, CancellationToken cancellationToken)
    {
        var userByEmail = await _userRepository.GetUserByEmail(request.Email);
        var userByUsername = await _userRepository.GetUserByUserName(request.Username);

        return new UserRegistrationPrecheckResponse() { EmailNotAvailable = userByEmail is not null, UserNameNotAvailable = userByUsername is not null };
    }
}
