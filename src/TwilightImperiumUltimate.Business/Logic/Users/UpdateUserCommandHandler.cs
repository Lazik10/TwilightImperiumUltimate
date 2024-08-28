namespace TwilightImperiumUltimate.Business.Logic.Users;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<UpdateUserCommand, TwilightImperiumUserDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<TwilightImperiumUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<TwilightImperiumUser>(request.User);
        var updatedUser = await _userRepository.UpdateUser(user);
        return _mapper.Map<TwilightImperiumUserDto>(updatedUser);
    }
}
