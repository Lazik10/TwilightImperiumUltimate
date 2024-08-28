
namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetUserByEmailQueryHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<GetUserByEmailQuery, TwilightImperiumUserDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<TwilightImperiumUserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);
        return _mapper.Map<TwilightImperiumUserDto>(user);
    }
}
