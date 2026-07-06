using TwilightImperiumUltimate.Contracts.ApiContracts.User;

namespace TwilightImperiumUltimate.Business.Logic.Users;

public class CheckRegistrationInfoQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<CheckRegistrationInfoQuery, UserRegistrationPrecheckResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserRegistrationPrecheckResponse> Handle(CheckRegistrationInfoQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userByEmail = await _userRepository.GetUserByEmail(request.Email).ConfigureAwait(false);
        var userByUsername = await _userRepository.GetUserByUserName(request.Username).ConfigureAwait(false);

        return new UserRegistrationPrecheckResponse() { EmailNotAvailable = userByEmail is not null, UserNameNotAvailable = userByUsername is not null };
    }
}
