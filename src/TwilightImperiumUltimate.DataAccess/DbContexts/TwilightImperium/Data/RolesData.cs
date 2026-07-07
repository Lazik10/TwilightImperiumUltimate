namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RolesData
{
    internal static List<IdentityRole> Roles => new()
    {
        new() { Id = "2147411d-19b7-4936-800a-b8d815271d00", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d01" },
        new() { Id = "cc4089b0-22e9-47df-b7c5-a4734b4423f4", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d02" },
        new() { Id = "5b2bee5c-e5ce-4472-a141-bff7e040ac78", Name = "Moderator", NormalizedName = "MODERATOR", ConcurrencyStamp = "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d03" },
        new() { Id = "d3f1c4e2-3f4a-4e2b-8f4e-2c3b5e6d7f89", Name = "TiglAdmin", NormalizedName = "TIGLADMIN", ConcurrencyStamp = "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d04" },
    };
}
