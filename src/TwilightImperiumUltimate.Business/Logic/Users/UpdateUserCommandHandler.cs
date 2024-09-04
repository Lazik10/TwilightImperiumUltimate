using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.Users;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<UpdateUserCommand, ApiResponse<TwilightImperiumUserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<TwilightImperiumUserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<TwilightImperiumUser>(request.User);
        var updateSuccessful = await _userRepository.UpdateUser(user);

        if (updateSuccessful)
            return new ApiResponse<TwilightImperiumUserDto>() { Success = updateSuccessful, Data = request.User };

        return new ApiResponse<TwilightImperiumUserDto>() { Success = updateSuccessful, Data = null, ProblemDetails = new ProblemDetailsDto() { Title = "Update failed", } };
    }
}
