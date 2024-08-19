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
            Id = Guid.NewGuid().ToString(),
            UserName = "TestUser",
            NormalizedUserName = "TESTUSER",
            Email = "Test@user.cz",
            NormalizedEmail = "TEST@USER.CZ",
            EmailConfirmed = true,
            FirstName = "Test",
            LastName = "User",
            FavoriteFaction = FactionName.TheYssarilTribes,
            UserInfo = "First seeded test user",
        };

        var passwordHasher = new PasswordHasher<TwilightImperiumUser>();
        var hashedPassword = passwordHasher.HashPassword(user, "testpassword");
        user.PasswordHash = hashedPassword;

        return user;
    }
}
