namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetAllUsersQueryHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<GetAllUsersQuery, ItemListDto<TwilightImperiumUserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<TwilightImperiumUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsers();
        var usersDto = _mapper.Map<List<TwilightImperiumUserDto>>(users);
        return new ItemListDto<TwilightImperiumUserDto>(usersDto);
    }
}
