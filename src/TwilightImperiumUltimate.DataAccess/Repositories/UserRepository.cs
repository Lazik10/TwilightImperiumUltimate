namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class UserRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IUserRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<IReadOnlyCollection<TwilightImperiumUser>> GetAllUsers()
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        return await dbContext.Users.ToListAsync();
    }

    public async Task<TwilightImperiumUser?> GetUserByEmail(string email)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        return await dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<TwilightImperiumUser?> GetUserById(string id)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        return await dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateUser(TwilightImperiumUser user)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        var dbUser = await dbContext.Users.FindAsync(user.Id);

        if (dbUser is null)
            return false;

        UpdateDbUser(dbUser, user);
        dbContext.Update(dbUser);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public void UpdateDbUser(TwilightImperiumUser dbUser, TwilightImperiumUser user)
    {
        dbUser.UserName = user.UserName;
        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.Email = user.Email;
        dbUser.Age = user.Age;
        dbUser.PhoneNumber = user.PhoneNumber;
        dbUser.UserInfo = user.UserInfo;
        dbUser.FavoriteFaction = user.FavoriteFaction;
        dbUser.DiscordId = user.DiscordId;
        dbUser.SteamId = user.SteamId;
    }

    public async Task<IReadOnlyCollection<IdentityRole>> GetAllRoles()
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        return await dbContext.Roles.ToListAsync();
    }

    public async Task<List<string?>> GetpecificUserRoles(string userId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var userRoleIds = await dbContext.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId)
            .ToListAsync();

        var roles = await dbContext.Roles
            .ToListAsync();

        return roles.Where(x => userRoleIds.Contains(x.Id)).Select(x => x.Name).ToList();
    }

    public async Task<bool> AddUserToRole(string userId, string roleName)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var roleId = await dbContext.Roles
            .Where(x => x.Name == roleName)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (roleId == null)
            return false;

        var userRole = new IdentityUserRole<string>
        {
            UserId = userId,
            RoleId = roleId,
        };

        dbContext.UserRoles.Add(userRole);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserFromRole(string userId, string roleName)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var roleId = await dbContext.Roles
            .Where(x => x.Name == roleName)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (roleId == null)
            return false;

        var userRole = new IdentityUserRole<string>
        {
            UserId = userId,
            RoleId = roleId,
        };

        dbContext.UserRoles.Remove(userRole);
        await dbContext.SaveChangesAsync();

        return true;
    }
}
