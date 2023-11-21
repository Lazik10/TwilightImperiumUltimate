using TwilightImperiumUltimate.DataAccess.RelationshipEntities;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContext : DbContext
{
    public TwilightImperiumDbContext(DbContextOptions<TwilightImperiumDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        modelBuilder.Entity<FactionTechnology>()
            .HasKey(ft => new { ft.FactionName, ft.TechnologyName });
    }
}
