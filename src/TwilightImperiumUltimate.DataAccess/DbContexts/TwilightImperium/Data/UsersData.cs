using TwilightImperiumUltimate.Core.Entities.Users;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class UsersData
{
    internal static List<User> Users =>
    [
        new()
        {
            FirstName = "Admin",
            LastName = "Admin",
            Email = "LazikL@email.cz",
            Nickname = "Lazik",
            Age = 32,
        },
    ];
}
