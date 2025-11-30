namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RolesData
{
    internal static List<IdentityRole> Roles => new()
    {
        new() { Id = "2147411d-19b7-4936-800a-b8d815271d00", Name = "Admin", NormalizedName = "ADMIN" },
        new() { Id = "cc4089b0-22e9-47df-b7c5-a4734b4423f4", Name = "User", NormalizedName = "USER" },
        new() { Id = "5b2bee5c-e5ce-4472-a141-bff7e040ac78", Name = "Moderator", NormalizedName = "MODERATOR" },
        new() { Id = "d3f1c4e2-3f4a-4e2b-8f4e-2c3b5e6d7f89", Name = "TiglAdmin", NormalizedName = "TIGLADMIN" },
    };
}
