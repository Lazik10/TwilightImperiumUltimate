namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetAllUserRolesQuery(string id)
    : IRequest<ItemListDto<RoleDto>>
{
    public string Id { get; set; } = id;
}
