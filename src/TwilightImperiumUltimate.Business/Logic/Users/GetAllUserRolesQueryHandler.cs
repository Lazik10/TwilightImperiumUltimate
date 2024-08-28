namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetAllUserRolesQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<GetAllUserRolesQuery, ItemListDto<RoleDto>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ItemListDto<RoleDto>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await _userRepository.GetpecificUserRoles(request.Id);

        var userRolesStrings = new List<RoleDto>();

        foreach (var role in userRoles.Where(x => x is not null))
        {
            userRolesStrings.Add(new RoleDto(role!));
        }

        return new ItemListDto<RoleDto>(userRolesStrings);
    }
}
