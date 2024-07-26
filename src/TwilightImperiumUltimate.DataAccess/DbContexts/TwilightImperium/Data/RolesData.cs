namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RolesData
{
    internal static List<IdentityRole> Roles => new()
    {
        new() { Name = "Admin", NormalizedName = "ADMIN" },
        new() { Name = "User", NormalizedName = "USER" },
        new() { Name = "Moderator", NormalizedName = "MODERATOR" },
    };
}
