using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContext(DbContextOptions<TwilightImperiumDbContext> options)
    : IdentityDbContext<TwilightImperiumUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        _ = builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
    }
}
