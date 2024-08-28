namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<GetUserByIdQuery, TwilightImperiumUserDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<TwilightImperiumUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.Id);
        if (user is null)
            user = new TwilightImperiumUser();

        return _mapper.Map<TwilightImperiumUserDto>(user);
    }
}
