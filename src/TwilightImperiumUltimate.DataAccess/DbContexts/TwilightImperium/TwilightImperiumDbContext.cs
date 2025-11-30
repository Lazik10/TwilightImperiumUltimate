using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContext(DbContextOptions<TwilightImperiumDbContext> options)
    : IdentityDbContext<TwilightImperiumUser>(options)
{
    public DbSet<DiscordRoleChangeLog> DiscordRoleChangeLogs => Set<DiscordRoleChangeLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        _ = builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        this.SeedDatabaseAsync(builder);
    }
}
