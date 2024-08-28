namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetAllRolesQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<GetAllRolesQuery, ItemListDto<RoleDto>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ItemListDto<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _userRepository.GetAllRoles();
        var roleStrings = roles
            .Where(x => x.Name is not null)
            .Select(r => new RoleDto(r.Name!))
            .ToList();

        return new ItemListDto<RoleDto>(roleStrings);
    }
}
