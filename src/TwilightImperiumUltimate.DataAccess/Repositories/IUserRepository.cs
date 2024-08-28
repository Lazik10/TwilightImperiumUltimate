namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyCollection<TwilightImperiumUser>> GetAllUsers();

    Task<TwilightImperiumUser?> GetUserById(string id);

    Task<TwilightImperiumUser?> GetUserByEmail(string email);

    Task<bool> UpdateUser(TwilightImperiumUser user);

    Task<IReadOnlyCollection<IdentityRole>> GetAllRoles();

    Task<List<string?>> GetpecificUserRoles(string userId);

    Task<bool> AddUserToRole(string userId, string roleName);

    Task<bool> DeleteUserFromRole(string userId, string roleName);
}
