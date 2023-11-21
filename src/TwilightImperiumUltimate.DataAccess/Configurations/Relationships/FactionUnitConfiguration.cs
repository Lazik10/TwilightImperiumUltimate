using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

public class FactionUnitConfiguration : IEntityTypeConfiguration<FactionUnit>
{
    public void Configure(EntityTypeBuilder<FactionUnit> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Configure the composite key for the intermediary table
        builder.HasKey(fu => new { fu.FactionName, fu.UnitName });

        // Configure the many-to-many relationship for Faction
        builder.HasOne(fu => fu.Faction)
               .WithMany(f => f.FactionUnits)
               .HasForeignKey(fu => fu.FactionName);

        // Configure the many-to-many relationship for Unit
        builder.HasOne(fu => fu.Unit)
               .WithMany(u => u.FactionUnits)
               .HasForeignKey(fu => fu.UnitName);
    }
}
