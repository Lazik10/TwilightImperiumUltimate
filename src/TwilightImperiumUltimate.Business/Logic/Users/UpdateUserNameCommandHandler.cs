namespace TwilightImperiumUltimate.Business.Logic.Users;

public class UpdateUserNameCommandHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<UpdateUserNameCommand, TwilightImperiumUserDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<TwilightImperiumUserDto> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.UpdateInitialUserName(request.Email, request.UserName);
        return _mapper.Map<TwilightImperiumUserDto>(dbUser);
    }
}
