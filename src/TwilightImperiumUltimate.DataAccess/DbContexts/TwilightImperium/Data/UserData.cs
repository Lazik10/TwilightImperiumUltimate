namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class UserData
{
    internal static List<TwilightImperiumUser> Users =>
    [
        CreateUser(),
    ];

    internal static TwilightImperiumUser CreateUser()
    {
        var user = new TwilightImperiumUser()
        {
            Id = "1",
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            Email = "Test@user.cz",
            NormalizedEmail = "TEST@USER.CZ",
            EmailConfirmed = true,
            FirstName = "Test",
            LastName = "User",
            FavoriteFaction = FactionName.TheYssarilTribes,
            UserInfo = "First seeded test user",
            ConcurrencyStamp = "b3f1533b-9a81-4b4e-9717-4ed06275b455",
            PasswordHash = "AQAAAAIAAYagAAAAEBR3YUVjbEJ90H/W4UJvhWxkzFGKDcfkvuXqznAhS6vj0p8vBUgFBKepfxoHMw2wmA==",
            SecurityStamp = "9ccae6b7-e015-4b21-8b07-6d4d85b6192b",
        };

        return user;
    }
}
